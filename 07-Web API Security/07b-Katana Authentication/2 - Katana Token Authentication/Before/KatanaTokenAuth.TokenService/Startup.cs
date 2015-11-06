using KatanaTokenAuth.TokenService;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KatanaTokenAuth.TokenService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Add error, welcome pages
            app.UseErrorPage();
            app.UseWelcomePage();
        }
    }
}
