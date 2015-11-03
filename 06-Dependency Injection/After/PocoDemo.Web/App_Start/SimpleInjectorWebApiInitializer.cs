using System.Web.Http;
using PocoDemo.Data;
using PocoDemo.Patterns.EF.Repositories;
using PocoDemo.Patterns.EF.UnitOfWork;
using PocoDemo.Patterns.Repositories;
using PocoDemo.Patterns.UnitOfWork;
using PocoDemo.Web;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SimpleInjectorWebApiInitializer), "Initialize")]

namespace PocoDemo.Web
{
    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? Go to: https://bit.ly/YE8OJj.
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            // Register services
            container.RegisterWebApiRequest<NorthwindSlim>();
            container.RegisterWebApiRequest<IProductsRepository, ProductsRepository>();
            container.RegisterWebApiRequest<ICustomersRepository, CustomersRepository>();
            container.RegisterWebApiRequest<IOrdersRepository, OrdersRepository>();
            container.RegisterWebApiRequest<INorthwindUnitOfWork, NorthwindUnitOfWork>();
        }
    }
}