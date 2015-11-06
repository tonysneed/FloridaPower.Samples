using System.Web.Http;

namespace KatanaBasicAuth.Controllers
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
