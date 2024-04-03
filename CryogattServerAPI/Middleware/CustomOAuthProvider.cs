using Infrastructure.Users;
using Infrastructure.Users.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using Unity;

namespace CryogattServerAPI.Middleware
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext _context)
        {
            ApplicationUser user = null;

            var UserManager = UnityConfig.Container.Resolve<IUserManager>();
            // Validate user credentials
            user = UserManager.ValidateUser(_context.UserName, _context.Password);
            if (user == null)
            {
                _context.SetError("Invalid username or password.", "The username or password is incorrect.");
                return Task.FromResult<object>(null);
            }

            AuthenticationTicket ticket = new AuthenticationTicket(SetClaimsIdentity(_context, user), new AuthenticationProperties());
            _context.Validated(ticket);

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext _context)
        {
            _context.Validated();
            return Task.FromResult<object>(null);
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext _context, ApplicationUser _user)
        {
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, _context.UserName));
            identity.AddClaim(new Claim("sub", _context.UserName));

            foreach (string role in _user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return identity;
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                string allowHeader = context.Request.Headers.Get("Access-Control-Request-Headers");
                if (!string.IsNullOrEmpty(allowHeader))
                {
                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { allowHeader });
                }
                context.OwinContext.Response.Headers.Add("access-control-allow-credentials", new[] { "true" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }
    }
}