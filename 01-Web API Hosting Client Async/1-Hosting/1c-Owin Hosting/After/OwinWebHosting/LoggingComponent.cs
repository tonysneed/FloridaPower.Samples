using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OwinWebHosting
{
    public class LoggingComponent
    {
        private Func<IDictionary<string, object>, Task> _next;

        public LoggingComponent(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            // Log request and response info
            Debug.Print("Request Path: {0}", environment["owin.RequestPath"]);

            // Invoke next component in the chain of middleware
            await _next(environment);

            // Log response
            Debug.Print("Response Status Code: {0}", environment["owin.ResponseStatusCode"]);
            
            // Return blank task
            //return Task.FromResult<object>(null);
        }
    }
}