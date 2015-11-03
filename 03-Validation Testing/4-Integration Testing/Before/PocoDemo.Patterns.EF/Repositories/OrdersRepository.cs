using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PocoDemo.Data;
using PocoDemo.Patterns.Repositories;

namespace PocoDemo.Patterns.EF.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly NorthwindSlim _dbContext;

        public OrdersRepository(NorthwindSlim dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Order>> GetOrders(string customerId)
        {
            var orders = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrder(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                .SingleOrDefaultAsync(o => o.OrderId == id);
            return order;
        }

        public async Task CreateOrder(Order order)
        {
            foreach (var detail in order.OrderDetails)
            {
                var detail1 = detail;
                detail.Product = await _dbContext.Products.SingleAsync(
                    p => p.ProductId == detail1.ProductId);
            }
            _dbContext.Orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            // Retrieve existing order
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails) // Include child entities
                .SingleOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return false;

            // Remove details in reverse order
            for (int i = order.OrderDetails.Count - 1; i > -1; i--)
            {
                var detail = order.OrderDetails.ElementAt(i);
                _dbContext.OrderDetails.Remove(detail);
            }
            _dbContext.Orders.Remove(order);
            return true;
        }

        public async Task<bool> OrderExists(int id)
        {
            bool exists = await _dbContext.Orders.AnyAsync(o => o.OrderId == id);
            return exists;
        }
    }
}
