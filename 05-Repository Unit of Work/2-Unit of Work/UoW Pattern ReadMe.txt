Unit of Work Pattern Demo

This demonstrates how to implement the Unit of Work pattern.
While the OrdersRepository could expose a SaveChanges method,
we may need to save changes across multiple respositories.
Unit of Work allows us to do just that.

1. Start by refactoring the customers and products repositories
   - Remove the using block from each method
     > Add a _dbContext field of type NorthwindSlim
     > Add a ctor with a dbContext parameter, set _dbContext to it
   - Update customer and products controllers with _dbContext field
     > Pass to repo ctor

2. We're going to start by adding a UnitOfWork interface to
   the Patterns project.
   - First add a UnitOfWork folder
   - Add an INorthwindUnitOfWork interface
     > Add three read-only properties, one for each repo interface

    IProductsRepository ProductsRepository { get; }
    ICustomersRepository CustomersRepository { get; }
    IOrdersRepository OrdersRepository { get; }

	> Add a SaveChangesAsync method that returns a Task<int>

3. Next implement the unit of work interface in the Patterns.EF project
   - Add a UnitOfWork folder
   - Add a NorthwindUnitOfWork class:

    public class NorthwindUnitOfWork : INorthwindUnitOfWork
    {
        private readonly NorthwindSlim _dbContext;
        private readonly IProductsRepository _productsRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IOrdersRepository _ordersRepository;

        public NorthwindUnitOfWork()
        {
            _dbContext = new NorthwindSlim();
            _productsRepository = new ProductsRepository(_dbContext);
            _customersRepository = new CustomersRepository(_dbContext);
            _ordersRepository = new OrdersRepository(_dbContext);
        }

        public IProductsRepository ProductsRepository
        {
            get { return _productsRepository; }
        }

        public ICustomersRepository CustomersRepository
        {
            get { return _customersRepository; }
        }

        public IOrdersRepository OrdersRepository
        {
            get { return _ordersRepository; }
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }

4. Refactor each controller to use INorthwindUnitOfWork
   - Initialize INorthwindUnitOfWork to a new NorthwindUnitOfWork in the ctor
   - You can now remove the _dbContext field from each controller
   - Update code to use repositories which hang off unit of work
   - Replace calls to _dbContext with _unitOfWork

5. Update INorthwindUnitOfWork to inherit from IDisposable
   - Update NorthwindUnitOfWork by adding a Dispose method
     in which you call _dbContext.Dispose
   - Update code in Dispose method of OrdersController to call
     _unitOfWork.Dispose
   - Copy the Dipose method from OrdersController to the customers
     and products controllers

   - Build the solution, then run service and client to test