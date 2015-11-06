using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace KatanaBasicAuth.Authentication
{
    public class BasicAuthenticationMiddleware : AuthenticationMiddleware<BasicAuthenticationOptions>
    {
        public BasicAuthenticationMiddleware(OwinMiddleware next, 
            BasicAuthenticationOptions options) : base(next, options) { }

        protected override AuthenticationHandler<BasicAuthenticationOptions> CreateHandler()
        {
            return new BasicAuthenticationHandler();
        }
    }
}