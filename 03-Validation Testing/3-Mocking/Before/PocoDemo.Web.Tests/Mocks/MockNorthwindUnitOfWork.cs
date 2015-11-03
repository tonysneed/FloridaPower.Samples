using System;
using System.Threading.Tasks;
using PocoDemo.Patterns.Repositories;
using PocoDemo.Patterns.UnitOfWork;

namespace PocoDemo.Web.Tests.Mocks
{
    public class MockNorthwindUnitOfWork : INorthwindUnitOfWork
    {
        public MockNorthwindUnitOfWork(IProductsRepository productsRepository)
        {
            ProductsRepository = productsRepository;
        }

        public IProductsRepository ProductsRepository { get; private set; }

        public ICustomersRepository CustomersRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IOrdersRepository OrdersRepository
        {
            get { throw new NotImplementedException(); }
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}