Dependency Injection Demo

To make our service more flexible we need to decouple it from
infrastructure concerns, such as a data access API (Entity Framework).
This will also allow us to test the service without external
dependencies.

1. First refactor NorthwindUnitOfWork in the Patterns.EF project
   - Instead of new-ing up the NorthwindSlim DbContext, we can
     accept it as a ctor parameter

2. Next refactor each controller to accept as a parameter an
   INorthwindUnitOfWork
   - Instead of new-ing up the UoW, we'll set the field to the
     ctor parameter

   - If you test the service, you'll see an exception that the
     controller does not have a parameterless ctor

3. We're going to use SimpleInjector as our DI container
   - Add the SimpleInjector.Integration.WebApi package to the Web project
   - Then add SimpleInjector.Integration.WebApi.WebHost.QuickStart
     > This adds a SimpleInjectorWebApiInitializer class to App_Start,
	   which uses Web Activator to bootstrap a SimpleInjector container.
   - Add code to the InitializeContainer method which registers the
     NorthwindSlim class, and also the repository and unit of work classes.

    private static void InitializeContainer(Container container)
    {
        // Register services
        container.RegisterWebApiRequest<NorthwindSlim>();
        container.RegisterWebApiRequest<IProductsRepository, ProductsRepository>();
        container.RegisterWebApiRequest<ICustomersRepository, CustomersRepository>();
        container.RegisterWebApiRequest<IOrdersRepository, OrdersRepository>();
        container.RegisterWebApiRequest<INorthwindUnitOfWork, NorthwindUnitOfWork>();
    }

	- Run service and client to test