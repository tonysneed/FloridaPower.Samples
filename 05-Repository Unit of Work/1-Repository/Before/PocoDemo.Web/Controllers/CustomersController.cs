using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PocoDemo.Data;

namespace PocoDemo.Web.Controllers
{
    public class CustomersController : ApiController
    {
        // GET api/customers
        [ResponseType(typeof(IEnumerable<Customer>))]
        public async Task<IHttpActionResult> Get()
        {
            using (var dbContext = new NorthwindSlim())
            {
                var customers = await dbContext.Customers
                    .OrderBy(p => p.CustomerId)
                    .ToListAsync();
                return Ok(customers);
            }
        }
    }
}
