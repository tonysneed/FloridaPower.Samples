using System.Collections.Generic;
using System.Web.Http;
using ModelBinding.Web.Models;

namespace ModelBinding.Web.Controllers
{
    public class LocationsController : ApiController
    {
        private static readonly List<LocationInfo> Locations = new List<LocationInfo>();

        // GET: api/Location
        public IEnumerable<LocationInfo> Get()
        {
            return Locations;
        }

        // GET: api/Location/5
        public LocationInfo Get(int id)
        {
            return Locations[id - 1];
        }

        // POST: api/Location/1,2
        [Route("api/locations/{value}")]
        public void Post([FromUri]Location value)
        {
            Locations.Add(new LocationInfo { X = value.X, Y = value.Y });
        }

        // PUT: api/Location/5/1,2
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