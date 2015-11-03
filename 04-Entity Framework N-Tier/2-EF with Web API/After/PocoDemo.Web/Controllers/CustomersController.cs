using PocoDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;

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
