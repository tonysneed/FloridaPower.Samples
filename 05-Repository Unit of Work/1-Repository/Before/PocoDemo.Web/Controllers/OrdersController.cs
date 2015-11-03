using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PocoDemo.Data;

namespace PocoDemo.Web.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly NorthwindSlim _dbContext = new NorthwindSlim();

        // GET: api/Orders?customerId = 5
        [ResponseType(typeof(IEnumerable<Order>))]
        public async Task<IHttpActionResult> GetOrders(string customerId)
        {
            var orders = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                .SingleOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Populate detail products
            foreach (var detail in order.OrderDetails)
            {
                var detail1 = detail;
                detail.Product = await _dbContext.Products.SingleAsync(
                    p => p.ProductId == detail1.ProductId);
            }

            _dbContext.Orders.Add(order);

            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(OrderDetail))]
        public async Task<IHttpActionResult> PutOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Entry(order).State = EntityState.Modified;

            Exception exception = null;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException updateEx)
            {
                exception = updateEx;
            }
            if (exception != null)
            {
                if (!await OrderExists(order.OrderId))
                    return NotFound();
                throw exception;
            }

            return Ok(order);
        }

        // DELETE: api/Orders/5
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            // Retrieve existing order
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails) // Include child entities
                .SingleOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return Conflict();

            // Remove details in reverse order
            for (int i = order.OrderDetails.Count - 1; i > -1; i--)
            {
                var detail = order.OrderDetails.ElementAt(i);
                _dbContext.OrderDetails.Remove(detail);
            }
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<bool> OrderExists(int id)
        {
            return await _dbContext.Orders.AnyAsync(e => e.OrderId == id);
        }
    }
}