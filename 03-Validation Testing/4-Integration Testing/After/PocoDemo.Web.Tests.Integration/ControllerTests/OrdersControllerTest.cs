using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using AspnetWebApi2Helpers.Serialization;
using PocoDemo.Data;
using PocoDemo.Patterns.EF.Repositories;
using PocoDemo.Patterns.EF.UnitOfWork;
using PocoDemo.Patterns.Repositories;
using PocoDemo.Patterns.UnitOfWork;
using PocoDemo.Web.Controllers;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Xunit;

namespace PocoDemo.Web.Tests.Integration.ControllerTests
{
    public class OrdersControllerTest
    {
        [Fact]
        public async void GetShouldReturnOrderWithOrderId_1()
        {
            // Arrange

            // Setup Http Config
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi", 
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});
            config.Formatters.JsonPreserveReferences();

            // Setup DI container
            var container = new Container();
            const string databasePath = @"..\..\..\PocoDemo.Web\App_Data\NorthwindSlim.mdf";
            string connectionString = string.Format(
                    @"Data Source=(localdb)\v11.0;AttachDbFilename={0};Integrated Security=True;MultipleActiveResultSets=True",
                    Path.GetFullPath(databasePath));
            container.RegisterWebApiRequest(() => new NorthwindSlim(connectionString));
            container.RegisterWebApiRequest<IProductsRepository, ProductsRepository>();
            container.RegisterWebApiRequest<INorthwindUnitOfWork, NorthwindUnitOfWork>();
            container.RegisterWebApiRequest<ProductsController>();
            container.Verify();

            // Set Dependency Resolver
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            // Create server and client
            var server = new HttpServer(config);
            var client = new HttpClient(server);
            client.BaseAddress = new Uri("http://test.com/api/orders/");
            const int orderId = 1;

            // Act
            HttpResponseMessage response = await client.GetAsync(orderId.ToString());

            // Assert
            response.EnsureSuccessStatusCode();
            var order = await response.Content.ReadAsAsync<Order>();
            Assert.NotNull(order);
            Assert.Equal(1, order.OrderId);
        }
    }
}
