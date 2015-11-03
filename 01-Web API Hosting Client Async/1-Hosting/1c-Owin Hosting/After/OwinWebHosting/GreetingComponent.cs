using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OwinWebHosting
{
    public class GreetingComponent
    {
        private Func<IDictionary<string, object>, Task> _next;

        public GreetingComponent(Func<IDictionary<string, object>, Task> next)
        {
            // Get pointer to next component in the pipeline
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            // Get response stream and write to it
            var response = (Stream)environment["owin.ResponseBody"];
            using (var writer = new StreamWriter(response))
                return writer.WriteAsync("Hello from Owin!");
        }
    }
}