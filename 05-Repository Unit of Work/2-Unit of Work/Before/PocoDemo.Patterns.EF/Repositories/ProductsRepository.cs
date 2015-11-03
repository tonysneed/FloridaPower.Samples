using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PocoDemo.Data;
using PocoDemo.Patterns.Repositories;

namespace PocoDemo.Patterns.EF.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        public async Task<IEnumerable<Product>> GetProducts()
        {
            using (var db = new NorthwindSlim())
            {
                var products = await db.Products
                    .Include(p => p.Category)
                    .OrderBy(p => p.ProductName)
                    .ToListAsync();
                return products;
            }
        }
    }
}
