using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Agile.Diagnostics.Logging;

namespace SecureWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Logger.Debug("WebApiConfig.Register");
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MessageHandlers.Add(new CustomMessageHandler());
        }
    }
    public class CustomMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Logger.Debug("Request: {0}", request.RequestUri.AbsoluteUri);
            if(request.Headers?.Authorization != null) // DELETE THIS
                Logger.Debug(request.Headers.Authorization.Parameter);

            // Call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
