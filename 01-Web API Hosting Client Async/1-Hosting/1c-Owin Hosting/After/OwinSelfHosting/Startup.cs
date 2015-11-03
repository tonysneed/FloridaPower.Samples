using System;
using System.Web.Http;
using Owin;

namespace OwinSelfHosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Show an error page
            app.UseErrorPage();

            // Logging component
            app.Use<LoggingComponent>();

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

            // Throw an exception
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.ToString() == "/fail")
                    throw new Exception("Doh!");
                await next();
            });

            // Welcome page
            app.UseWelcomePage("/");
        }
    }
}
