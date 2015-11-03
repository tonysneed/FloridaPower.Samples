using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PocoDemo.Data;
using PocoDemo.Patterns.EF.Repositories;
using PocoDemo.Patterns.Repositories;

namespace PocoDemo.Web.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly NorthwindSlim _dbContext = new NorthwindSlim();
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController()
        {
            _ordersRepository = new OrdersRepository(_dbContext);
        }

        // GET: api/Orders?customerId = 5
        [ResponseType(typeof(IEnumerable<Order>))]
        public async Task<IHttpActionResult> GetOrders(string customerId)
        {
            var orders = await _ordersRepository.GetOrders(customerId);
            return Ok(orders);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            var order = await _ordersRepository.GetOrder(id);

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

            await _ordersRepository.CreateOrder(order);

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

            _ordersRepository.UpdateOrder(order);

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
                if (!await _ordersRepository.OrderExists(order.OrderId))
                    return NotFound();
                throw exception;
            }

            return Ok(order);
        }

        // DELETE: api/Orders/5
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            // Retrieve existing order
            if (!await _ordersRepository.DeleteOrder(id))
                return Conflict();

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
    }
}