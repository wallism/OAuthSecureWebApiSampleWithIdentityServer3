using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using Agile.Diagnostics.Logging;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;

namespace AuthorizationServer
{
    public interface ISampleIdentityManager
    {
        IEnumerable<Client> GetClients();
        IEnumerable<Scope> GetScopes();
        List<InMemoryUser> GetInMemoryUsers();
    }

    public class SampleInMemoryManager : ISampleIdentityManager
    {
        private const string SecureWebApiSampleScope = "SecureWebApiSampleScope";

        public IEnumerable<Client> GetClients()
        {
            Logger.Debug(MethodBase.GetCurrentMethod().Name);
            return new List<Client>
            {
                new Client
                {
                    ClientId = "myclientid",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("bigsecret".Sha256())
                    },
                    ClientName = "my client name",
                    // seconds the token will be valid for
                    AccessTokenLifetime = 3600, // 1 hour (which is the default...but not necessarily recommended)
                    Flow = Flows.ResourceOwner,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Address,
                        SecureWebApiSampleScope,
                        "UndefinedScope"
                    }
                }
            };
        }

        public IEnumerable<Scope> GetScopes()
        {
            Logger.Debug(MethodBase.GetCurrentMethod().Name);
            return new []
            {
                StandardScopes.Address,
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = SecureWebApiSampleScope,
                    DisplayName = "Read User Data"
                }, 
            };
        }

        public List<InMemoryUser> GetInMemoryUsers()
        {
            Logger.Debug(MethodBase.GetCurrentMethod().Name);
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "mark.wallis@rockend.com.au",
                    Username = "mark.wallis@rockend.com.au",
                    Password = "securepassword",
                    Claims = new [] { new Claim(Constants.ClaimTypes.Name, "Mark Wallis"), }
                }
            };
        }
    }
}