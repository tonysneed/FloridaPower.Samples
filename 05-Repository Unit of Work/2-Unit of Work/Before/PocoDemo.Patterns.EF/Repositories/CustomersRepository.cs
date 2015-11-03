using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PocoDemo.Data;
using PocoDemo.Patterns.Repositories;

namespace PocoDemo.Patterns.EF.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            using (var dbContext = new NorthwindSlim())
            {
                var customers = await dbContext.Customers
                    .OrderBy(p => p.CustomerId)
                    .ToListAsync();
                return customers;
            }
        }
    }
}
