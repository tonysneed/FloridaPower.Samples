Mocking Framework Demo

This demo shows how to use a mocking framework to test Web API controllers.
We are able to use mocks because we can designed our controllers to be testable
by using interfaces and declaring our dependencies via ctor injection.

1. Refactor ProductsControllerTest to use Moq
   - Update the Arrange part to use Moq
   - Then delete the Mocks folder and its content

    // Mock products repo
    var productsRepoMock = new Mock<IProductsRepository>();
    productsRepoMock.Setup(m => m.GetProducts()).ReturnsAsync(expected);

    // Mock unit of work
    var unitofWorkMock = new Mock<INorthwindUnitOfWork>();
	unitofWorkMock.SetupGet(m => m.ProductsRepository).Returns(productsRepoMock.Object);

    // Create controller
    var productsController = new ProductsController(unitofWorkMock.Object);

	- Build, then run the tests, which should pass.

2. Add an OrdersControllerTest class to the ControllerTests folder
   in the Tests project.
   - Add a test method:
   
   [Fact]
   public async void PostOrderShouldCreateNewOrder()
   
   - Create a new order:

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

3. Mock orders repo, unit of work
   - Note that orders mock does not require setup,
     because the CreateOrder method just sets entity state.

    // Mock orders repo
    var ordersRepoMock = new Mock<IOrdersRepository>();

    // Mock unit of work
    var unitOfWorkMock = new Mock<INorthwindUnitOfWork>();
    unitOfWorkMock.SetupGet(m => m.OrdersRepository).Returns(ordersRepoMock.Object);

	- Set up the SaveChangesAsync method
	  > Should return number of rows affected: 1
	  > Should set Order.OrderId to 1

    unitOfWorkMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1)
        .Callback(() => order.OrderId = 1);

   - Create controller

   var controller = new OrdersController(unitOfWorkMock.Object);

   - Act: Call PostOrder

   IHttpActionResult response = await controller.PostOrder(order);

4. Assert expected result
   - Cast response to CreatedAtRouteNegotiatedContentResult
   - Assert order.OrderId is 1
   - Assert id route value is same as order.OrderId

    // Assert
    var result = (CreatedAtRouteNegotiatedContentResult<Order>) response;
    Assert.Equal(1, order.OrderId);
    Assert.Equal(order.OrderId, result.RouteValues["id"]);

	- Build and run tests