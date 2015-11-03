EF with Web API Demo

NOTE: Install SQL Express or LocalDb      
      Create NorthwindSlim database, run script

Part A: Reverse Engineer Additional Entities

1. Re-add Data Model to generate additional entities
   - Delete files Product.cs and Category.cs from Data project
     > To see them, show all files for the project
   - Add an ADO.NET Entity Data Model
     > Name it NorthwindSlim
     > Select Code First from Database
     > Select NorthwindSlim data connection
     > Uncheck option to save connection string
     > Select Customer, Order, OrderDetail, Product, Category
     
 2. Link added classes from the Entities project
    - Add existing items, navigate to Data project
    - Add As Link: Customer.cs, Order.cs, OrderDetail.cs
    - Exclude entities from the Data project
    - Build solution
    
Part B: Customers and Orders Controllers

1. Add a Customers controller to the Web project
   - Right-click Controllers folder, Add Controller
     > Select Web API 2 Controller - Empty
   - Add Get method returning customers sorted by CustomerId
   
	// GET api/customers
	[ResponseType(typeof(IEnumerable<Customer>))]
	public async Task<IHttpActionResult> Get()
	{
		using (var dbContext = new NorthwindSlim())
		{
			var customers = await dbContext.Customers
				.OrderBy(p => p.CustomerId)
				.ToListAsync();
			return Ok(customers);
		}
	}
	
2. Refactor client to retreive customers
   - In client.GetAsync, change products to customers
   - In ReadAsync, change Product to Customer
     > Also update foreach loop
   - Test client with updated web service
     
3. Add Orders Controller
   - Select Web API 2 Controller with actions using EF
     > Model class: Order
     > Data context class: NorthwindSlim
     > Check use async controller actions
     > Controller name: OrdersController
     
4. Refactor the Orders Controller
   - Rename the NorthwindSlim db field to _dbContext
   - Replace the Get method as follows:
   
	// GET: api/Orders?customerId = 5
	[ResponseType(typeof(IEnumerable<Order>))]
	public async Task<IHttpActionResult> Get(string customerId)
	{
		var orders = await _dbContext.Orders
			.Include(o => o.Customer)
			.Include("OrderDetails.Product")
			.Where(o => o.CustomerId == customerId)
			.ToListAsync();
		return Ok(orders);
	}

- Replace GetOrder as follows:

	// GET: api/Orders/5
	[ResponseType(typeof(Order))]
	public async Task<IHttpActionResult> GetOrder(int id)
	{
		var order = await _dbContext.Orders
			.Include(o => o.Customer)
			.Include("OrderDetails.Product")
			.SingleOrDefaultAsync(o => o.OrderId == id);
		if (order == null) return NotFound();
		return Ok(order);
	}


5. Refactor PutOrder method
   - Change ResponseType from void to Order
   - Remove id parameter
     > Also remove code checking for incorrect id
     > Change id to orderId in catch block
   - Change return to Ok(order)
   
   
6. Refactor DeleteOrder method
   - Change ResponseType to void
   - Update code to retrieve order with details
   - Return Conflict if not found
   - Remove details in reverse order
   - Then remove order
   - Save changes
   - Return Ok()
   
