using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using KatanaBasicAuth.Models;

namespace KatanaBasicAuth.Controllers
{
    public class ClaimsController : ApiController
    {
        // GET: api/claims
        [ResponseType(typeof(IEnumerable<ClaimInfo>))]
        public IHttpActionResult Get()
        {
            // Cast user to claims principle
            var principle = User as ClaimsPrincipal;
            if (principle == null) return Ok();

            // Return claims
            var claims = principle.Claims.Select
                (c => new ClaimInfo {Type = c.Type, Value = c.Value});
            return Ok(claims);
        }
    }
}
