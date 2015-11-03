Integration Testing Demo

This demonstrates how to perform integration testing of a Web API service.
It uses the "real" repo implementations, so we'll configure a DI container.

1. Add a Web.Tests.Integration class library
   - Add NuGet packages:
     xunit, xunit.runner.visualstudio
	 Microsoft.AspNet.WebApi.Core
	 SimpleInjector.Integration.WebApi
	 EntityFramework
  - Reference Entities, Data, Patterns, Patterns.EF, Web projects
  - Add a ControllerTests folder

2. Update NorthwindSlim.Extensions.cs in the Data project to include
   a ctor that accepts a connection string.

    public NorthwindSlim(string nameOrConnectionString)
        : base(nameOrConnectionString)
    {
        Initialize();
    }

3. Add a public class OrdersControllerTest class to the ControllerTests folder
   - Add a test method:

   [Fact]
   public async void GetShouldReturnOrderWithOrderId_1()

4. Add code to arrange the test
   - Setup Http Config
     > Include code to configure Json serializer to preserve references,
	   so that it can handle cycles between objects.

    // Setup Http Config
    var config = new HttpConfiguration();
    config.Routes.MapHttpRoute(
        name: "DefaultApi", 
        routeTemplate: "api/{controller}/{id}",
        defaults: new {id = RouteParameter.Optional});
    config.Formatters.JsonPreserveReferences();

	- Setup DI container
	  > Build a connection string using LocalDb which includes a path to
	    NorthwindSlim.mdf in the App_Data folder of the Web project.

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

	- Set Dependency Resolver, create Http server and client

    // Set Dependency Resolver
    config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

    // Create server and client
    var server = new HttpServer(config);
    var client = new HttpClient(server);
    client.BaseAddress = new Uri("http://test.com/api/orders/");
    const int orderId = 1;

5. Insert code to Act and Assert

    // Act
    HttpResponseMessage response = await client.GetAsync(orderId.ToString());

    // Assert
    response.EnsureSuccessStatusCode();
    var order = await response.Content.ReadAsAsync<Order>();
    Assert.NotNull(order);
    Assert.Equal(1, order.OrderId);

	- Build and run the test
