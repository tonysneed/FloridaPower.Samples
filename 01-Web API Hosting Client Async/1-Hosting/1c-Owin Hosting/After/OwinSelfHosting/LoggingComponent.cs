using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinSelfHosting
{
    public class LoggingComponent : OwinMiddleware
    {
        public LoggingComponent(OwinMiddleware next) : base(next) { }

        public async override Task Invoke(IOwinContext context)
        {
            // Log request and response info
            Debug.Print("Request Path: {0}", context.Request.Path);

            // Invoke next component in the chain of middleware
            await Next.Invoke(context);

            // Log response
            Debug.Print("Response Status Code: {0}", context.Response.StatusCode);
        }
    }
}
