using System.Web.Http;

namespace OwinSelfHosting
{
    public class ValuesController : ApiController
    {
        // api/values
        public string[] Get()
        {
            return new[] { "Value1", "Value2", "Value3" };
        }
    }
}
