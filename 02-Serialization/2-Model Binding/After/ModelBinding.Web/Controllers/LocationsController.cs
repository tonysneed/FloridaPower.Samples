using System.Collections.Generic;
using System.Web.Http;
using ModelBinding.Web.Models;

namespace ModelBinding.Web.Controllers
{
    public class LocationsController : ApiController
    {
        private static readonly List<LocationInfo> Locations = new List<LocationInfo>();

        // GET: api/Locations
        public IEnumerable<LocationInfo> Get()
        {
            return Locations;
        }

        // GET: api/Locations/5
        public LocationInfo Get(int id)
        {
            return Locations[id - 1];
        }

        // POST: api/Locations/1,2
        // POST: api/Locations/top-right
        [Route("api/locations/{value}")]
        public void Post(Location value) // No need for [FromUri] with converter/binder
        {
            Locations.Add(new LocationInfo { X = value.X, Y = value.Y });
        }

        // PUT: api/Locations/5/1,2
        public void Put(int id, Location value)
        {
            Locations[id - 1].X = value.X;
            Locations[id - 1].Y = value.Y;
        }

        // DELETE: api/Location/5
        public void Delete(int id)
        {
            Locations.RemoveAt(id - 1);
        }
    }
}