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
        private readonly NorthwindSlim _dbContext;

        public ProductsRepository(NorthwindSlim dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _dbContext.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductName)
                .ToListAsync();
            return products;
        }
    }
}
