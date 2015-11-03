using System.Collections.Generic;
using System.Threading.Tasks;
using PocoDemo.Data;

namespace PocoDemo.Patterns.Repositories
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
