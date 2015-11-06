using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;

namespace AspNet5EmptyWeb
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            // Insert middleware into the pipeline for logging request and response
            app.Use(next => async context =>
            {
                Console.WriteLine("Request Path: {0}", context.Request.Path);
                await next.Invoke(context);
                Console.WriteLine("Response Status Code: {0}", context.Response.StatusCode);
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
