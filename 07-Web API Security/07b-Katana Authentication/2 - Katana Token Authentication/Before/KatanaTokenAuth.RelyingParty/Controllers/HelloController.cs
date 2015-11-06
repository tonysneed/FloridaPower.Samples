using System.Web.Http;

namespace KatanaTokenAuth.RelyingParty.Controllers
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
