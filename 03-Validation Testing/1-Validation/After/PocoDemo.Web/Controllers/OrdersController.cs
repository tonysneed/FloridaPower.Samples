using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PocoDemo.Data;
using PocoDemo.Patterns.UnitOfWork;

namespace PocoDemo.Web.Controllers
{
    //[ValidateModel]
    public class OrdersController : ApiController
    {
        private readonly INorthwindUnitOfWork _unitOfWork;

        public OrdersController(INorthwindUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Orders?customerId = 5
        [ResponseType(typeof(IEnumerable<Order>))]
        public async Task<IHttpActionResult> GetOrders(string customerId)
        {
            var orders = await _unitOfWork.OrdersRepository.GetOrders(customerId);
            return Ok(orders);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            var order = await _unitOfWork.OrdersRepository.GetOrder(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            await _unitOfWork.OrdersRepository.CreateOrder(order);

            await _unitOfWork.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(OrderDetail))]
        public async Task<IHttpActionResult> PutOrder(Order order)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _unitOfWork.OrdersRepository.UpdateOrder(order);

            Exception exception = null;
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException updateEx)
            {
                exception = updateEx;
            }
            if (exception != null)
            {
                if (!await _unitOfWork.OrdersRepository.OrderExists(order.OrderId))
                    return NotFound();
                throw exception;
            }

            return Ok(order);
        }

        // DELETE: api/Orders/5
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            // Retrieve existing order
            if (!await _unitOfWork.OrdersRepository.DeleteOrder(id))
                return Conflict();

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}