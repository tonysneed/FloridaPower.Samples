using System.Web.Http;
using KatanaTokenAuth.RelyingParty;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KatanaTokenAuth.RelyingParty
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
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
