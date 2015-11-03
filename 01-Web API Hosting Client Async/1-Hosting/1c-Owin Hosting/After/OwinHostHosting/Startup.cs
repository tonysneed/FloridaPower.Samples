using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace OwinHostHosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure web api routing
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Use web api middleware
            app.UseWebApi(config);
        }
    }
}
