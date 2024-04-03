using System;
using Owin;
using Microsoft.Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using System.Web.Configuration;
using System.Text;

using CryogattServerAPI.Middleware;
using CryogattServerAPI.Trace;
using CryogattServerAPI.Models;

namespace CryogattServerAPI
{
    public class Startup
    {
        public SymmetricSecurityKey signingKey;

        public void Configuration(IAppBuilder app)
        {
            Log.Debug("Invoked");

            try
            {
                HttpConfiguration config = new HttpConfiguration();

                app.UseCors(CorsOptions.AllowAll);
                ConfigureAuth(app);
                app.UseWebApi(config);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            Log.Debug("Invoked");

            try
            {
                string issuer = WebConfigurationManager.AppSettings["TokenIssuer"];
                byte[] secret = Encoding.ASCII.GetBytes(StringConstants.SecretKey);

                app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    AllowedAudiences = new[] { WebConfigurationManager.AppSettings["TokenAudience"] },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[] { new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret) }
                });

                app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
                {
                    AllowInsecureHttp = false,
                    TokenEndpointPath = new PathString("/api/v1/users/tokens"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                    Provider = new CustomOAuthProvider(),
                    AccessTokenFormat = new CustomJwtFormat(issuer)
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}