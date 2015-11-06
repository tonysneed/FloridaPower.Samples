using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace KatanaBasicAuth.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        protected async override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            // Get authorization header
            var authHeader = Request.Headers.Get("Authorization");
            if (string.IsNullOrEmpty(authHeader) 
                || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return null;

            // Get credentials
            string username, password;
            var token = authHeader.Substring("Basic ".Length).Trim();
            if (!TryGetCredentials(token, out username, out password))
                return null;

            // Validate credentials
            bool isValid = await Options.Validator(username, password);
            if (!isValid)
                return null;

            // Create claims identity
            var identity = new ClaimsIdentity(GetUserClaims(username),
                Options.AuthenticationType);

            // Create an authentication ticket
            return new AuthenticationTicket(identity, new AuthenticationProperties());
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            // Respond to unauthorized code with an authenticate challenge
            if (Response.StatusCode == 401)
            {
                var challenge = Helper.LookupChallenge(Options.AuthenticationType, 
                    Options.AuthenticationMode);
                if (challenge != null)
                {
                    Response.Headers.AppendValues("WWW-Authenticate", 
                        "Basic realm=" + Options.Realm);
                }
            }
            return Task.FromResult<object>(null);
        }

        private IEnumerable<Claim> GetUserClaims(string username)
        {
            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
            };
        }

        private bool TryGetCredentials(string token, out string username, out string password)
        {
            // Pull username and password from base 64 encoded credential
            username = null;
            password = null;
            string credential;
            try
            {
                credential = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            var colonPosition = credential.IndexOf(':');
            if (colonPosition == -1)
                return false;

            username = credential.Substring(0, colonPosition);
            password = credential.Substring(colonPosition + 1);
            return true;
        }
    }
}