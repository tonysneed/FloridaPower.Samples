using Owin;

//[assembly: OwinStartup(typeof(OwinWebHosting.Startup))]

namespace OwinWebHosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<LoggingComponent>();
            app.Use<GreetingComponent>();
        }
    }
}
