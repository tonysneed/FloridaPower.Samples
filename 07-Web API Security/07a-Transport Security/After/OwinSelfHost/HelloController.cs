using System.Web.Http;

namespace OwinSelfHost
{
    public class HelloController : ApiController
    {
        // GET: api/hello
        public IHttpActionResult Get()
        {
            return Ok("Hello Web API Self Host");
        }
    }
}
