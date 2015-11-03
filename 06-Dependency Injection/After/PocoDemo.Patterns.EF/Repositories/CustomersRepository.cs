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
        private readonly NorthwindSlim _dbContext;

        public CustomersRepository(NorthwindSlim dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customers = await _dbContext.Customers
                .OrderBy(p => p.CustomerId)
                .ToListAsync();
            return customers;
        }
    }
}
