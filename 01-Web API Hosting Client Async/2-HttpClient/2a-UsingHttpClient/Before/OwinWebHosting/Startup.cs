using System.Web.Http;
using Owin;

namespace OwinWebHosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Config Web API routing
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Use Web API middleware
            app.UseWebApi(config);
        }
    }
}
