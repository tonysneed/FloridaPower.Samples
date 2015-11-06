using System.Web.Http;
using KatanaTokenAuth.RelyingParty;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KatanaTokenAuth.RelyingParty
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Consume bearer tokens
            var options = new OAuthBearerAuthenticationOptions
            {
                // Authn Filter asks for authentication
                AuthenticationMode = AuthenticationMode.Passive
            };
            app.UseOAuthBearerAuthentication(options);

            // Configure web api routing
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            app.UseWebApi(config);

            // Add error, welcome pages
            app.UseErrorPage();
            app.UseWelcomePage();
        }
    }
}
