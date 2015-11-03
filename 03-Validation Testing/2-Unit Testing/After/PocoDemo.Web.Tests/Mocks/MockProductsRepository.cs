using System.Collections.Generic;
using System.Threading.Tasks;
using PocoDemo.Data;
using PocoDemo.Patterns.Repositories;

namespace PocoDemo.Web.Tests.Mocks
{
    public class MockProductsRepository : IProductsRepository
    {
        private readonly IEnumerable<Product> _products;

        public MockProductsRepository(IEnumerable<Product> products)
        {
            _products = products;
        }
        public Task<IEnumerable<Product>> GetProducts()
        {
            return Task.FromResult(_products);
        }
    }
}