using System;
using System.ComponentModel.DataAnnotations;

namespace PocoDemo.Entities.Validation
{
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
}