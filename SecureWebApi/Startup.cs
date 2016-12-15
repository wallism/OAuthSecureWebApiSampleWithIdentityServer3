using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup(typeof(SecureWebApi.Startup))]

namespace SecureWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Logger.Debug(MethodBase.GetCurrentMethod().Name);

            // configure OAuth jwt token authorization and verification
            var certificate = new X509Certificate2(Convert.FromBase64String("MIIDCDCCAfCgAwIBAgIQM8nQLThOc69Ck/mntYARQzANBgkqhkiG9w0BAQUFADAcMRowGAYDVQQDDBF3d3cubXljb21wYW55LmNvbTAeFw0xNjEyMTQxMDQxNTdaFw0yMTEyMTUxMDQxNTdaMBwxGjAYBgNVBAMMEXd3dy5teWNvbXBhbnkuY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2sn1EnqIu5cjCAGj/4XLZdjudxzd6Y7BUk/Z+h0BPzJ3NxvLJGOr5NwnT+7TxDjdJr/+2wzn8SMSDREFG7vUM+mYxGhQue5ot2tWmxlFCAj3i/1mrXCit5PJ6eeZAzHUQdZMDhqnrnCJy0XtaUZc6eJOV0YBMjB/AQpEHo6yBfQcDxlhRdNZlQhq3gfjG6j3wsNUSDyKgJS+a6f7xfZVGfv2jJq5OhqTigigeWxiqO3cDfliQB+9l87YEE6kKX1u33aI4q5JqqMDpSMjBlW6qEIixRsCxsyjk/M/NVJ54pA1FzJy4rvaTuNwihr2GLKfLMYRq7fwbXaGeW+3vhfOewIDAQABo0YwRDATBgNVHSUEDDAKBggrBgEFBQcDATAOBgNVHQ8BAf8EBAMCB4AwHQYDVR0OBBYEFF0veuEyCCo48hp62EKbsrfr7IOUMA0GCSqGSIb3DQEBBQUAA4IBAQB2DIT+hQhN7IUVHW2kkgw8BX8hh+laJdiFF99C9ZwFDaflsIyUVpgqJZIb2wRiwjAFA1HRsoidB/J0cAfmWyTO9OeEED/tGLLfxKUtlgkmucuA5eZSGIjswYlaAdEeWL58M/c9UOF5zK8vlgI7rsL8dYrLp3IRatFqUJRDJDDSmTT0ySLnoR1hv8gGT07j/P4bMHUNfp8S03jLeaJhNOy0on9HNbbzIarD/qeMVt2HUJYxDq4JnV6FRi6okYvkfjsWDUzOMJrJG7omzifP30GvKrbgmXLr+spHN3tomBbaFNSA4mudrLW1z1U/MPYNVyOEASD/3ewfRwulPGCiD3fa"));
            var options = new JwtBearerAuthenticationOptions
            {
                AllowedAudiences = new[] { "http://localhost:3817/resources" },
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = "http://localhost:3817/resources",
                    ValidIssuer = "http://localhost:3817",   // get from "issuer" in http://localhost:port/.well-known/openid-configuration

                    RequireExpirationTime = true,
                    IssuerSigningTokens = new List<SecurityToken>
                    {
                        new X509SecurityToken(certificate)
                    }

                    // I don't think the key is necessary, just the SigningToken(s). 
//                    IssuerSigningKey = new X509SecurityKey(certificate),
                }
            };

            app.UseJwtBearerAuthentication(options);


            // simplified way to verify the token IF we use IdentityServer (nuget package: IdentityServer3.AccessTokenValidation)
//            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
//            {
//                Authority = "http://localhost:3817"
//            });

        }
    }
}
