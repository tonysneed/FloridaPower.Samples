using System.Collections.Generic;
using System.Web.Http;

namespace OwinWebHosting
{
    public class PersonsController : ApiController
    {
        private static readonly List<Person> Persons = new List<Person>
            {
                new Person { Name = "Peter", Age = 30 },
                new Person { Name = "Paul", Age = 31 },
                new Person { Name = "Mary", Age = 32 },
            };

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
    }
}