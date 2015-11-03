using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UsingHttpClient
{
    class Program
    {
        // Note: append .fiddler to localhost for use with Fiddler.

        private const string LocalBaseAddress = "http://localhost:57989/api/";
        private static Uri _greetingsBaseUri = new Uri(LocalBaseAddress + "greetings/");
        private static Uri _personsBaseUri = new Uri(LocalBaseAddress + "persons/");

        static void Main(string[] args)
        {
        }
    }
}
