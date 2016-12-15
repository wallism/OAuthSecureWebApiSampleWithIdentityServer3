using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using StackExchange.Redis;

[assembly: OwinStartup(typeof(AuthorizationServer.Startup))]

namespace AuthorizationServer
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                Logger.Debug("Configure AuthorizationServer");

                var manager = new SampleInMemoryManager();

                var factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(manager.GetClients())
                    .UseInMemoryScopes(manager.GetScopes())
                    .UseInMemoryUsers(manager.GetInMemoryUsers());

                // Default caching
                factory.ConfigureClientStoreCache();
                factory.ConfigureScopeStoreCache();
                factory.ConfigureUserServiceCache(TimeSpan.FromMinutes(1));

                #region Redis caching
                //                var myRedisMultiplexInstance = ConnectionMultiplexer.Connect("...redis connection string...");

                //                var clientStoreCache = new ClientStoreCache(myRedisMultiplexInstance);
                //                var scopeStoreCache = new ScopeStoreCache(myRedisMultiplexInstance);
                //                var userServiceCache = new UserServiceCache(myRedisMultiplexInstance);
                //
                //                factory.ConfigureClientStoreCache(new Registration<ICache<Client>>(clientStoreCache));
                //                factory.ConfigureScopeStoreCache(new Registration<ICache<IEnumerable<Scope>>>(scopeStoreCache));
                //                factory.ConfigureUserServiceCache(new Registration<ICache<IEnumerable<Claim>>>(userServiceCache));
                #endregion

                // NOTE: there is a PostBuild event that will try to copy the file to the IIS express dir using:    xcopy ..\testSsl.pfx "C:\Program Files (x86)\IIS Express\" /C /R /Y
                var pfxPath = string.Format("{0}\\testSsl.pfx", Environment.CurrentDirectory); // change this to where works for you

                if (!File.Exists(pfxPath))
                    throw new Exception(string.Format("pfx should be here: {0}", pfxPath));

                // if the "Welcome to IdentityServer3" page does not show, check the password for your certificate
                var signingCertificate = new X509Certificate2(pfxPath, "CertSecurePassword", X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);
                var options = new IdentityServerOptions
                {
                    SigningCertificate = signingCertificate,
#if DEBUG
                    RequireSsl = false,
#else
            RequireSsl = true,
#endif     
                    Factory = factory
                };

                app.UseIdentityServer(options);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Configuration");
            }
        }
    }
}
