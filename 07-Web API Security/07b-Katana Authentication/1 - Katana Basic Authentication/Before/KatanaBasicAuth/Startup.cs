using System.Threading.Tasks;
using System.Web.Http;
using KatanaBasicAuth;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KatanaBasicAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // TODO: Use basic authentication middleware

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

        private Task<bool> Validator(string username, string password)
        {
            // NOTE: You'll want to validate base using a credentials repository

            // Valid if username and password are the same
            return Task.FromResult(username == password);
        }
    }
}
