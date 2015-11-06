using System.Web.Http;
using Owin;

namespace OwinSelfHost
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

            // Add welcome page
            app.UseWelcomePage();
        }
    }
}
