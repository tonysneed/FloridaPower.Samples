using System;
using FluentValidation;

namespace PocoDemo.Web.Validation
{
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
}