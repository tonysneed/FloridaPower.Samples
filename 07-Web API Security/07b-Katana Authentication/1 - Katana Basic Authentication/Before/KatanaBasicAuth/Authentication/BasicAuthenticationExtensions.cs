using System;
using System.Threading.Tasks;
using Owin;

namespace KatanaBasicAuth.Authentication
{
    public static class BasicAuthenticationExtensions
    {
        // Strongly typed middleware usage
        public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, 
            string realm, Func<string, string, Task<bool>> validator)
        {
            var options = new BasicAuthenticationOptions(realm, validator);
            return app.Use<BasicAuthenticationMiddleware>(options);
        }
    }
}