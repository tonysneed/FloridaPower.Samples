using System.Web.Http;

namespace WebHosting
{
    public class ValuesController : ApiController
    {
        [Route("api/values")]
        public string[] Get()
        {
            return new[] {"Value1", "Value2", "Value3"};
        }
    }
}