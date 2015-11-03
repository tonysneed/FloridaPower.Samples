using FluentValidation;
using FluentValidation.Results;
using PocoDemo.Data;

namespace PocoDemo.Web.Validation
{
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
}