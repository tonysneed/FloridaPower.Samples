using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using PocoDemo.Data;
using PocoDemo.Patterns.Repositories;
using PocoDemo.Patterns.UnitOfWork;
using PocoDemo.Web.Controllers;
using PocoDemo.Web.Tests.Mocks;
using Xunit;

namespace PocoDemo.Web.Tests.ControllerTests
{
    public class ProductsControllerTest
    {
        [Fact]
        public async void GetShouldReturnProducts()
        {
            // Arrange
            var expected = new List<Product>
            {
                new Product {ProductId = 1, ProductName = "Product1"},
                new Product {ProductId = 2, ProductName = "Product2"},
                new Product {ProductId = 3, ProductName = "Product3"},
            };
            IProductsRepository productsRepo = new MockProductsRepository(expected);
            INorthwindUnitOfWork unitOfWork = new MockNorthwindUnitOfWork(productsRepo);
            var productsController = new ProductsController(unitOfWork);

            // Act
            IHttpActionResult response = await productsController.Get();

            // Assert
            var actual = ((OkNegotiatedContentResult<IEnumerable<Product>>)response).Content;
            var comparer = new GenericComparer<Product>(
                (p1, p2) => p1.ProductId == p2.ProductId);
            Assert.Equal(expected, actual, comparer);
        }
    }
}
