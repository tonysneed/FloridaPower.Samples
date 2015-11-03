Validation Demo

Here we'll demonstrate how to perform data validation of client requests.

1. Observe entity validation
   - Open the Order class and notice the StringLength of 5
   - Open OrdersController, Post method, and set a breakpoint
   - Modify the client to pass a CustomerId of "ABCDEF"
   - Launch web project in the debugger, then start the client
   - Notice how the service returns a 400 Bad Request status
   - Add a Range(10, 100) to Order.Freight, then test client with 1

2. Next add a custom validation attribute to check for negative values
   - Add a Validation folder to the Entities project
   - Add a NonNegativeAttribute class, derive from ValidationAttribute

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NonNegativeAttribute : ValidationAttribute 
    {
        // Set error message
        public NonNegativeAttribute()
            : base("Property should be non-negative.") { }

        // Check if value is negative integer
        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            if ((dynamic)value < 0)
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName));
            return null;
        }
    }

   - Add [NonNegative] attribute to Orders.Frieght
     > Also comment out the [Range] attribute
     > Test by setting Freight to a negative number in the Client

3. Add the fluent validation package for Web API to the Web project
   - NuGet package: FluentValidation.WebAPI
   - Add a Validation folder with a OrderValidator class
     > Derive from AbstractValidator<Order>
	 > Add a ctor with validation rules
   - Add rules:
     > Freight between 0 and 100
	 > Ship date may not preceed order date

    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            // Freight between 0 and 100
            RuleFor(x => x.Freight).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);

            // Ship date may not preceed order date
            Custom(x =>
            {
                if (x.ShippedDate < x.OrderDate)
                {
                    return new ValidationFailure("ShippedDate",
                        "Shipped Date may not preceed Order Date.");
                }
                return null;
            });
        }
    }

4. Create a FluentValidatorFactory that derives from ValidatorFactoryBase
   - Accept an IServiceProvider in the ctor, then use to create the instance

    public class FluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly IServiceProvider _provider;

        public FluentValidatorFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            var validator = _provider.GetService(validatorType) as IValidator;
            return validator;
        }
    }

5. Update SimpleInjectorWebApiInitializer to register validators
   and to use the validator factory
   - Before the call to Verify, add validator registration code

    // Register validators
    container.RegisterManyForOpenGeneric(typeof(IValidator<>), typeof(OrderValidator).Assembly);

   - After Verify, add code to set the validator factory

    // Set validator factory
    FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration,
        provider => provider.ValidatorFactory = new FluentValidatorFactory(container));

   - Test with client setting negative value for Order.Freight
   - Then try setting the ship date to occur prior to the order date

6. Run the client with Fiddler to inspect the body of the Post response
   - Note that validation errors are present for both rule violations
   - If you were to pass an invalid CustomerId, note that the fluent
     validation does not take place.
	 > It is therefore recommended to use fluent validation for everything,
	   removing from the entity T4 template the code to insert property attributes.

7. Lastly, rather than checking the Model.State in each method that needs it,
   which can be duplicative and error-prone, create an ActionFilterAttribute
   - Add a ValidateModelAttribute class to the Web Validation folder
     > Extend ActionFilterAttribute and override OnActionExecuting

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request
                    .CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }

	- Then apply the [ValidateModel] attribute to the OrdersController class
	  > Comment out the code in Post and Put that checks ModelState

8. Instead of applying the ValidateModel attribute to individual controllers or actions,
   you can instead apply it globally by adding it to the Filters collection of
   HttpConfiguration.
   - Place the following code in WebApiConfig.Register
     before configuring routes

    // Add validation filter
    config.Filters.Add(new ValidateModelAttribute());

   - Comment out the attrbute you placed on the OrdersController
   - Then re-test the service and client using Fiddler

