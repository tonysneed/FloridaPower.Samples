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
    public class CustomersController : ApiController
    {
        private readonly ICustomersRepository _customersRepository;

        public CustomersController()
        {
            _customersRepository = new CustomersRepository();
        }

        // GET api/customers
        [ResponseType(typeof(IEnumerable<Customer>))]
        public async Task<IHttpActionResult> Get()
        {
            var customers = await _customersRepository.GetCustomers();
            return Ok(customers);
        }
    }
}
