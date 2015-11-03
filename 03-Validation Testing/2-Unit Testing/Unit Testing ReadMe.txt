Unit Testing Demo

Here we'll demonstrate unit testing of Web API controllers.
A class library project has been added with xUnit and Moq.

1. Add a ControllerTests folder
   - Reference Entities, Patterns, Web projects
   - Add a ProductsControllerTest class
   - Add a method: public void GetShouldReturnProducts()
   - Add a [Fact] attribute to the method

2. Implement a mock IProductsRepository
   - Add a Mocks folder to the test project
   - Add a MockProductsRepository class that implements IProductsRepository
   - Implement GetProducts by creating a List of three products
   - Return Task.FromResult 

    public class MockProductsRepository : IProductsRepository
    {
        private readonly IEnumerable<Product> _products;

        public MockProductsRepository(IEnumerable<Product> products)
        {
            _products = products;
        }
        public Task<IEnumerable<Product>> GetProducts()
        {
            return Task.FromResult(_products);
        }
    }

3. Implement a mock INorthwindUnitOfWork
   - Place in the Mocks folder
   - Just implement the ProductsRepository property

    public class MockNorthwindUnitOfWork : INorthwindUnitOfWork
    {
        public MockNorthwindUnitOfWork(IProductsRepository productsRepository)
        {
            ProductsRepository = productsRepository;
        }

        public IProductsRepository ProductsRepository { get; private set; }

		// Other members omitted ...
	}

4. Add a GenericComparer method
   - Add a ctor that accepts a parameter of Func<T, T, bool>

    public class GenericComparer<T> : IEqualityComparer<T> 
        where T : class
    {
        private readonly Func<T, T, bool> _comparer;

        public GenericComparer(Func<T, T, bool> comparer)
        {
            _comparer = comparer;
        }
        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }
        public int GetHashCode(T x)
        {
            return x.GetHashCode();
        }
    }

5. Flesh out the GetShouldReturnProducts method in ProductsControllerTest

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
        IProductsRepository productsRepo = new MockProductsRepository(products);
        INorthwindUnitOfWork unitOfWork = new MockNorthwindUnitOfWork(productsRepo);
        var productsController = new ProductsController(unitOfWork);

        // Act
        IHttpActionResult response = await productsController.Get();

        // Assert
        var actual = ((OkNegotiatedContentResult<IEnumerable<Product>>)response).Content;
        var comparer = new GenericComparer<Product>(
            (p1, p2) => p1.ProductId == p2.ProductId);
        Assert.Equal(products, actual, comparer);
    }

	- Build and run tests.