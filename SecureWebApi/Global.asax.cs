using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Agile.Diagnostics.Loggers;
using Agile.Diagnostics.Logging;

namespace SecureWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Logger.InitializeLogging(new List<ILogger> { new TraceLogger()}, LogLevel.All);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
