using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Text;

using CryogattServerAPI.Trace;
using System.Web.Configuration;

namespace CryogattServerAPI.Middleware
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        // Plain text secret
        private static readonly byte[] secret = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["SecretKey"]);

        // Issuer of Token
        private readonly string issuer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_issuer"></param>
        public CustomJwtFormat(string _issuer)
        {
            Log.Debug("Invoked");

            issuer = _issuer;
        }

        public string Protect(AuthenticationTicket _data)
        {
            Log.Debug("Invoked");

            if (_data == null)
            {
                throw new ArgumentNullException(nameof(_data));
            }
            InMemorySymmetricSecurityKey signingKey = new InMemorySymmetricSecurityKey(secret);
            System.IdentityModel.Tokens.SigningCredentials signingCredentials = new System.IdentityModel.Tokens.SigningCredentials(signingKey, System.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature, System.IdentityModel.Tokens.SecurityAlgorithms.Sha256Digest);
            DateTimeOffset? issued = _data.Properties.IssuedUtc;
            DateTimeOffset? expires = _data.Properties.ExpiresUtc;

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(issuer, WebConfigurationManager.AppSettings["TokenAudience"], _data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials));
        }

        public AuthenticationTicket Unprotect(string _protectedText)
        {
            Log.Debug("Invoked");

            throw new NotImplementedException();
        }
    }
}