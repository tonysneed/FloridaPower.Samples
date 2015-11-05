using System.Collections.Generic;
using System.Web.Http;
using ModelBinding.Web.Models;

namespace ModelBinding.Web.Controllers
{
    public class PersonsController : ApiController
    {
        private static readonly Dictionary<int, Person> Persons =
            new Dictionary<int, Person>()
        {
            { 1, new Person { Name = "Peter", Age = 20} },
            { 2, new Person { Name = "Paul", Age = 30} },
            { 3, new Person { Name = "Mary", Age = 40} },
        };

        // GET: api/Person
        public IEnumerable<Person> Get()
        {
            return Persons.Values;
        }

        // GET: api/Person/5
        public Person Get(int id)
        {
            return Persons[1];
        }

        // POST: api/Person/5?Name=Peter&Age=20
        public void Post(int id, [FromUri]Person value)
        {
            Persons.Add(id, value);
        }

        // PUT: api/Person/5??Name=Peter&Age=20
        public void Put(int id, [FromUri]Person value)
        {
            Persons[id] = value;
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
            Persons.Remove(id);
        }
    }
}
