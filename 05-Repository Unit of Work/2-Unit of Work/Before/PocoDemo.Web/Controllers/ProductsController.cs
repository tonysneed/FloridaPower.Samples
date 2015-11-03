using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PocoDemo.Data;
using PocoDemo.Patterns.EF.Repositories;
using PocoDemo.Patterns.Repositories;

namespace PocoDemo.Web.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController()
        {
            _productsRepository = new ProductsRepository();
        }

        [ResponseType(typeof(IEnumerable<Product>))]
        public async Task<IHttpActionResult> Get()
        {
            var products = await _productsRepository.GetProducts();
            return Ok(products);
        }
    }
}
