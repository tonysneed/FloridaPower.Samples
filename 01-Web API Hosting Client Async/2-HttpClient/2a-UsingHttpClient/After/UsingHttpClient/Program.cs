using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UsingHttpClient
{
    class Program
    {
        // Note: append .fiddler to localhost for use with Fiddler.

        private const string LocalBaseAddress = "http://localhost:57988/api/";
        private static Uri _greetingsBaseUri = new Uri(LocalBaseAddress + "greetings/");
        private static Uri _personsBaseUri = new Uri(LocalBaseAddress + "persons/");

        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to get greetings as JSON");
            Console.ReadLine();

            // Get greetings as JSON
            var greetingsJson = GetGreetingsJson().Result;
            Console.WriteLine(greetingsJson);

            Console.WriteLine("\nPress Enter to get greetings as XML");
            Console.ReadLine();

            // Get greetings as XML
            var greetingsXml = GetGreetingsXml().Result;
            Console.WriteLine(greetingsXml);

            Console.WriteLine("\nEnter id to get greeting as JSON");
            int id = int.Parse(Console.ReadLine());

            // Get greeting by id as JSON
            var greetingJson = GetGreetingByIdJson(id).Result;
            Console.WriteLine(greetingJson);

            Console.WriteLine("\nPress Enter to get greeting by invalid id");
            Console.ReadLine();

            // Get greetings as JSON
            var greetingsJson2 = GetGreetingByInvalidIdJson(10).Result;
            Console.WriteLine(greetingsJson2);
        }

        static async Task<string> GetGreetingsJson()
        {
            var client = new HttpClient {BaseAddress = _greetingsBaseUri};
            return await client.GetStringAsync("");
        }

        static async Task<string> GetGreetingsXml()
        {
            var client = new HttpClient{ BaseAddress = _greetingsBaseUri };
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            return await client.GetStringAsync("");
        }

        static async Task<string> GetGreetingByIdJson(int id)
        {
            var client = new HttpClient { BaseAddress = _greetingsBaseUri };
            return await client.GetStringAsync(id.ToString());
        }

        static async Task<string> GetGreetingByInvalidIdJson(int id)
        {
            var client = new HttpClient { BaseAddress = _greetingsBaseUri };
            HttpResponseMessage response = await client.GetAsync(id.ToString());
            //if (!response.IsSuccessStatusCode) return string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
