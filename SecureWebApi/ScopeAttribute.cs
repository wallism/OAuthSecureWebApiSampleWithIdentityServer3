using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Agile.Diagnostics.Logging;

namespace SecureWebApi
{
    /// <summary>
    /// Restrict access to a resource by scopes
    /// </summary>
    /// <remarks>Taken from: https://github.com/IdentityModel/Thinktecture.IdentityModel.45.git </remarks>
    public class ScopeAttribute : AuthorizeAttribute
    {
        string[] _scopes;
        static string _scopeClaimType = "scope";

        public static string ScopeClaimType
        {
            get { return _scopeClaimType; }
            set { _scopeClaimType = value; }
        }

        public ScopeAttribute(params string[] scopes)
        {
            if (scopes == null)
            {
                throw new ArgumentNullException("scopes");
            }

            _scopes = scopes;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var grantedScopes = ClaimsPrincipal.Current.FindAll(_scopeClaimType).Select(c => c.Value).ToList();

            if (grantedScopes.Count == 0)
            {
                Logger.Warning("grantedScopes=0. token may be expired!"); // expired token still comes in here for some reason, if we can figure out how to prevent that elsewhere in the pipeline that would be better
                return false;
            }

            foreach (var scope in _scopes)
            {
                if (!grantedScopes.Contains(scope))
                {
                    Logger.Warning("request does not have required scope(s): {0}", string.Join(" ", _scopes));
                    return false;
                }
            }

            return true;
        }
    }
}