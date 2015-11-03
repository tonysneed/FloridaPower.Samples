using System.Collections.Generic;
using System.Threading.Tasks;
using PocoDemo.Data;

namespace PocoDemo.Patterns.Repositories
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetOrders(string customerId);

        Task<Order> GetOrder(int id);

        Task CreateOrder(Order order);

        void UpdateOrder(Order order);

        Task<bool> DeleteOrder(int id);

        Task<bool> OrderExists(int id);
    }
}
