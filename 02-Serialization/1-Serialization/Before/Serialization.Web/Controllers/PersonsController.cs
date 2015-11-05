using System.Collections.Generic;
using System.Web.Http;
using Serialization.Web.Models;

namespace Serialization.Web.Controllers
{
    public class PersonsController : ApiController
    {
        private static readonly List<Person> Persons = new List<Person>
        {
            new Person { Name = "Peter", Age = 20},
            new Person { Name = "Paul", Age = 30 },
            new Person { Name = "Mary", Age = 40 },
        };

        public PersonsController()
        {
            //Persons[0].Brother = Persons[1];
            //Persons[1].Brother = Persons[0];
        }

        // GET: api/Person
        public IEnumerable<Person> Get()
        {
            return Persons;
        }

        // GET: api/Person/5
        public Person Get(int id)
        {
            return Persons[id - 1];
        }

        // POST: api/Person
        public void Post(Person value)
        {
            Persons.Add(value);
        }

        // PUT: api/Person/5
        public void Put(int id, Person value)
        {
            Persons[id - 1] = value;
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
            Persons.RemoveAt(id - 1);
        }
    }
}
