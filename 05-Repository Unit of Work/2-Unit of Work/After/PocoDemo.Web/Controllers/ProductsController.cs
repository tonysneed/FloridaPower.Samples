using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PocoDemo.Data;
using PocoDemo.Patterns.EF.UnitOfWork;
using PocoDemo.Patterns.UnitOfWork;

namespace PocoDemo.Web.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly INorthwindUnitOfWork _unitOfWork;

        public ProductsController()
        {
            _unitOfWork = new NorthwindUnitOfWork();
        }

        [ResponseType(typeof(IEnumerable<Product>))]
        public async Task<IHttpActionResult> Get()
        {
            var products = await _unitOfWork.ProductsRepository.GetProducts();
            return Ok(products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
