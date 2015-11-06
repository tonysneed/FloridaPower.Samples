using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace KatanaTokenAuth.TokenService.Providers
{
    public class DemoAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        // Credential validator
        public Func<string, string, Task<bool>> Validator { get; private set; }

        public DemoAuthorizationServerProvider
            (Func<string, string, Task<bool>> validator)
        {
            Validator = validator;
        }

        public async override Task GrantResourceOwnerCredentials
            (OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Validate credentials
            if (!await Validator(context.UserName, context.Password))
            {
                context.Rejected();
                return;
            }

            // Create identity
            var identity = new ClaimsIdentity(GetUserClaims(context.UserName),
                context.Options.AuthenticationType);

            // Create ticket
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            context.Validated(ticket);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // OAuth2 has notion of client authentication, which is not used here
            context.Validated();
            return Task.FromResult<object>(null);
        }

        private IEnumerable<Claim> GetUserClaims(string username)
        {
            return new[]
            {
                new Claim("sub", username),
                new Claim(ClaimTypes.Name, username),
            };
        }
    }
}