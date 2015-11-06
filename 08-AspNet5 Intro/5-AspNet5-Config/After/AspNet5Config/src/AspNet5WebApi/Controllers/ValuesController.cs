using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.OptionsModel;
using AspNet5WebApi.Properties;
using Microsoft.AspNet.Hosting;

namespace AspNet5WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        IHostingEnvironment _env;
        IOptions<Greeting> _greetingConfig;

        public ValuesController(IHostingEnvironment env,
            IOptions<Greeting> greetingConfig)
        {
            _env = env;
            _greetingConfig = greetingConfig;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var greeting = _greetingConfig.Options.DefaultGreeting;
            if (_env.IsEnvironment("Development"))
            {
                greeting = _greetingConfig.Options.WesternGreeting;
            }
            return greeting;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
