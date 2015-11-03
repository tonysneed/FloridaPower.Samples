using System;
using System.Threading.Tasks;
using PocoDemo.Patterns.Repositories;

namespace PocoDemo.Patterns.UnitOfWork
{
    public interface INorthwindUnitOfWork : IDisposable
    {
        IProductsRepository ProductsRepository { get; }
        ICustomersRepository CustomersRepository { get; }
        IOrdersRepository OrdersRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
