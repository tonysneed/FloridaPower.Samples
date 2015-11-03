using System.Collections.Generic;
using System.Threading.Tasks;
using PocoDemo.Data;

namespace PocoDemo.Patterns.Repositories
{
    public interface ICustomersRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
    }
}
