using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace SelfHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            // Web API routes
            var config = new HttpSelfHostConfiguration("http://localhost:12345");
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove xml formatter to return only JSON
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
