using System;
using System.Threading.Tasks;
using KatanaTokenAuth.TokenService;
using KatanaTokenAuth.TokenService.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KatanaTokenAuth.TokenService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Use oauth authz server to issue tokens
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true, // Set to false for production
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                Provider = new DemoAuthorizationServerProvider(Validator)
            });

            // Add error, welcome pages
            app.UseErrorPage();
            app.UseWelcomePage();
        }

        private Task<bool> Validator(string username, string password)
        {
            // NOTE: You'll want to validate base using a credentials repository

            // Valid if username and password are the same
            return Task.FromResult(username == password);
        }
    }
}
