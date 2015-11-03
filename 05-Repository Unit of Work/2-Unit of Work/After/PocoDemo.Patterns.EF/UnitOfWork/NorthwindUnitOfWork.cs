using System.Threading.Tasks;
using PocoDemo.Data;
using PocoDemo.Patterns.EF.Repositories;
using PocoDemo.Patterns.Repositories;
using PocoDemo.Patterns.UnitOfWork;

namespace PocoDemo.Patterns.EF.UnitOfWork
{
    public class NorthwindUnitOfWork : INorthwindUnitOfWork
    {
        private readonly NorthwindSlim _dbContext;
        private readonly IProductsRepository _productsRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IOrdersRepository _ordersRepository;

        public NorthwindUnitOfWork()
        {
            _dbContext = new NorthwindSlim();
            _productsRepository = new ProductsRepository(_dbContext);
            _customersRepository = new CustomersRepository(_dbContext);
            _ordersRepository = new OrdersRepository(_dbContext);
        }

        public IProductsRepository ProductsRepository
        {
            get { return _productsRepository; }
        }

        public ICustomersRepository CustomersRepository
        {
            get { return _customersRepository; }
        }

        public IOrdersRepository OrdersRepository
        {
            get { return _ordersRepository; }
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
