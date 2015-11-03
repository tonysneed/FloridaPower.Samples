using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OwinWebHosting
{
    public class GreetingsController : ApiController
    {
        private static readonly List<string> Greetings = new List<string>
        {
            "Hello",
            "Howdy",
            "Aloha",
            "Ciao"
        };

        // GET api/values
        public IEnumerable<string> Get()
        {
            return Greetings;
        }

        // GET api/values/1
        public IHttpActionResult Get(int id)
        {
            if (Greetings.Count < id)
                return NotFound();
            return Ok(Greetings[id - 1]);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            Greetings.Add(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            Greetings[id - 1] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            Greetings.RemoveAt(id - 1);
        }
    }
}