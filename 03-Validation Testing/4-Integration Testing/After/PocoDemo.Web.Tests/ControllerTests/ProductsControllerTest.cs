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
    public class ProductsControllerTest
    {
        [Fact]
        public async void GetShouldReturnProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product {ProductId = 1, ProductName = "Product1"},
                new Product {ProductId = 2, ProductName = "Product2"},
                new Product {ProductId = 3, ProductName = "Product3"},
            };

            // Mock products repo
            var productsRepoMock = new Mock<IProductsRepository>();
            productsRepoMock.Setup(m => m.GetProducts()).ReturnsAsync(products);

            // Mock unit of work
            var unitofWorkMock = new Mock<INorthwindUnitOfWork>();
            unitofWorkMock.SetupGet(m => m.ProductsRepository).Returns(productsRepoMock.Object);

            // Create controller
            var productsController = new ProductsController(unitofWorkMock.Object);

            // Act
            IHttpActionResult response = await productsController.Get();

            // Assert
            var actual = ((OkNegotiatedContentResult<IEnumerable<Product>>)response).Content;
            var comparer = new GenericComparer<Product>(
                (p1, p2) => p1.ProductId == p2.ProductId);
            Assert.Equal(products, actual, comparer);
        }
    }
}
