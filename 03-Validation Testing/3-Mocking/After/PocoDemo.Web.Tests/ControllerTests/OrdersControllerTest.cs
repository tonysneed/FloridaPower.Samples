using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using PocoDemo.Data;
using PocoDemo.Patterns.Repositories;
using PocoDemo.Patterns.UnitOfWork;
using PocoDemo.Web.Controllers;
using Xunit;

namespace PocoDemo.Web.Tests.ControllerTests
{
    public class OrdersControllerTest
    {
        [Fact]
        public async void PostOrderShouldCreateNewOrder()
        {
            // Arrange
            var order = new Order
            {
                CustomerId = "ALFKI",
                OrderDate = DateTime.Today,
                ShippedDate = DateTime.Today.AddDays(1),
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, Quantity = 5, UnitPrice = 10 },
                    new OrderDetail { ProductId = 2, Quantity = 10, UnitPrice = 20 },
                    new OrderDetail { ProductId = 4, Quantity = 40, UnitPrice = 40 }
                }
            };

            // Mock orders repo
            var ordersRepoMock = new Mock<IOrdersRepository>();

            // Mock unit of work
            var unitOfWorkMock = new Mock<INorthwindUnitOfWork>();
            unitOfWorkMock.SetupGet(m => m.OrdersRepository).Returns(ordersRepoMock.Object);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1)
                .Callback(() => order.OrderId = 1);

            // Create controller
            var controller = new OrdersController(unitOfWorkMock.Object);

            // Act
            IHttpActionResult response = await controller.PostOrder(order);

            // Assert
            var result = (CreatedAtRouteNegotiatedContentResult<Order>) response;
            Assert.Equal(1, order.OrderId);
            Assert.Equal(order.OrderId, result.RouteValues["id"]);
        }
    }
}
