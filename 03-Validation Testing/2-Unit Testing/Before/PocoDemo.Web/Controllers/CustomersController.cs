using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PocoDemo.Data;
using PocoDemo.Patterns.UnitOfWork;

namespace PocoDemo.Web.Controllers
{
    public class CustomersController : ApiController
    {
        private readonly INorthwindUnitOfWork _unitOfWork;

        public CustomersController(INorthwindUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/customers
        [ResponseType(typeof(IEnumerable<Customer>))]
        public async Task<IHttpActionResult> Get()
        {
            var customers = await _unitOfWork.CustomersRepository.GetCustomers();
            return Ok(customers);
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
