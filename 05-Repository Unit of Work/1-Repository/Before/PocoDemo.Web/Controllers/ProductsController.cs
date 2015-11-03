using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using PocoDemo.Data;

namespace PocoDemo.Web.Controllers
{
    public class ProductsController : ApiController
    {
        [ResponseType(typeof(IEnumerable<Product>))]
        public async Task<IHttpActionResult> Get()
        {
            using (var db = new NorthwindSlim())
            {
                var products = await db.Products
                    .Include(p => p.Category)
                    .OrderBy(p => p.ProductName)
                    .ToListAsync();
                return Ok(products);
            }
        }
    }
}
