using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace ModelBinding.Web.Models
{
    public class LocationTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, 
            Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, 
            CultureInfo culture, object value)
        {
            var input = value as string;
            if (input != null)
            {
                Location location;
                if (Location.TryParse(input, out location)) return location;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}