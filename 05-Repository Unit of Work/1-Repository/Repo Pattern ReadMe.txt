Repository Pattern Demo

Here we're going to decouple the Web API service from Entity Framework
by implementing the Repository pattern.

1. Add a class library project called PocoDemo.Patterns
   - Reference the Entities project
   - Add a Repositories folder
   - Add IXxxRepository interfaces with method signatures matching
     data access of the corresponding controller actions

	IProductsRepository
	- Task<IEnumerable<Product>> GetProducts

	ICustomersRepository
	- Task<IEnumerable<Customer>> GetCustomers

2. Add another class library project called PocoDemo.Patterns.EF
   - Add the EntityFramework NuGet package
   - Add a Repositories folder
   - Reference the Data, Entities and Patterns projects
   - Add implementation for ICustomersRepository, IProductsRepository
     > Add async keyword after 'public' in each method
	 > Copy and paste code from controller actions
	 > Remove Ok method to return entities

3. Update Customers and Products controllers in the Web project
   - Reference the Patterns and Patterns.EF projects
   - Add an IXxxRepository field to each controller
   - Add a ctor which initializes repo using new keyword
   - In actions, replace entire using block with call to
     repository GetXxx method

   - Build solution, then browse to Web API service to test both
     the customers and products controllers

4. Create an IOrdersRespository interface in the Patterns project

 	IOrdersRepository
    - Task<IEnumerable<Order>> GetOrders(string customerId);
    - Task<Order> GetOrder(int id);
    - Task CreateOrder(Order order);
    - void UpdateOrder(Order order);
    - Task<bool> DeleteOrder(int id);
	- Task<bool> OrderExists(int id);

5. Implement the Orders repository
   - Add a readonly field: NorthwindSlim _dbContext
   - Implement each method by coping code from OrdersController actions
   - Add async to each method returning a Task
   - For Delete, return false instead of Conflict
     > At the end return true
   - Don't include code for saving changes
     > This will be implemented by the Unit of Work later

6. Refactor OrdersController to use IOrdersRepository
   - Rename _dbContext to _ordersRepository
   - Replace NorthwindSlim with IOrdersRepository
     > Init to new OrdersRepository in ctor

    private readonly NorthwindSlim _dbContext = new NorthwindSlim();
    private readonly IOrdersRepository _ordersRepository;

    public OrdersController()
    {
        _ordersRepository = new OrdersRepository(_dbContext);
    }

   - Remove OrderExists method, call from ordersRepo in Put method
   - Call SaveChangesAsync from _dbContext
   - Dispose _dbContext

   - Build the solution
   - Run the service and client to test