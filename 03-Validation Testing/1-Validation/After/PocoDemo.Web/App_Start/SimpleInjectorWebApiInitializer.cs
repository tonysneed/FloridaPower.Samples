using System.Web.Http;
using FluentValidation;
using FluentValidation.WebApi;
using PocoDemo.Data;
using PocoDemo.Patterns.EF.Repositories;
using PocoDemo.Patterns.EF.UnitOfWork;
using PocoDemo.Patterns.Repositories;
using PocoDemo.Patterns.UnitOfWork;
using PocoDemo.Web;
using PocoDemo.Web.Validation;
using SimpleInjector;
using SimpleInjector.Extensions;
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

            // Register controllers
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            // Register validators
            container.RegisterManyForOpenGeneric(typeof(IValidator<>), typeof(OrderValidator).Assembly);

            // Verify registrations
            container.Verify();

            // Set validator factory
            FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration,
                provider => provider.ValidatorFactory = new FluentValidatorFactory(container));

            // Set dependency resolver
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