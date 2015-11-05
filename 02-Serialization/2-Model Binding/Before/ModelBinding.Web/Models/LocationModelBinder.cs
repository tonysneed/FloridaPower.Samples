using System;
using System.Collections.Concurrent;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace ModelBinding.Web.Models
{
    public class LocationModelBinder : IModelBinder
    {
        private static readonly ConcurrentDictionary<string, Location> LocationsCache =
            new ConcurrentDictionary<string, Location>(StringComparer.OrdinalIgnoreCase);

        static LocationModelBinder()
        {
            LocationsCache["bottom-left"] = new Location { X = 0, Y = 0 };
            LocationsCache["bottom-right"] = new Location { X = 100, Y = 0 };
            LocationsCache["top-left"] = new Location { X = 0, Y = 100 };
            LocationsCache["top-right"] = new Location { X = 100, Y = 100 };
        }

        public bool BindModel(HttpActionContext actionContext, 
            ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof (Location)) return false;
            ValueProviderResult input = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var key = input.RawValue as string;
            if (key == null) return false;

            Location location;
            if (LocationsCache.TryGetValue(key, out location)
                || Location.TryParse(key, out location))
            {
                bindingContext.Model = location;
                return true;
            }
            return false;
        }
    }
}