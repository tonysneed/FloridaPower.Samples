using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace UsingHttpClient
{
    class Program
    {
        // Note: append .fiddler to localhost for use with Fiddler.

        private const string LocalBaseAddress = "http://localhost:57990/api/";
        private static Uri _greetingsBaseUri = new Uri(LocalBaseAddress + "greetings/");
        private static Uri _personsBaseUri = new Uri(LocalBaseAddress + "persons/");

        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to get greetings as JSON");
            Console.ReadLine();

            // Get greetings as JSON
            var greetingsJson = GetGreetingsJson().Result;
            Console.WriteLine(greetingsJson);

            Console.WriteLine("\nAdd new greeting as JSON:");
            string greeting = Console.ReadLine();
            PostGreetingJson(greeting).Wait();

            Console.WriteLine("\nAdd new greeting as StringContent:");
            string greeting2 = Console.ReadLine();
            PostGreetingContent(greeting2).Wait();

            // TODO: Continue ...
        }

        static async Task<string> GetGreetingsJson()
        {
            var client = new HttpClient {BaseAddress = _greetingsBaseUri};
            return await client.GetStringAsync("");
        }

        static async Task PostGreetingJson(string greeting)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync
                (_greetingsBaseUri, greeting);
            response.EnsureSuccessStatusCode();
        }

        private static async Task PostGreetingContent(string greeting)
        {
            var client = new HttpClient();
            var content = new StringContent("\"" + greeting + "\"",
                Encoding.Default, "application/json");
            HttpResponseMessage response = await client.PostAsync
                (_greetingsBaseUri, content);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
        }

        private static void UseReadAsAsync(int id)
        {
            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync
                (_personsBaseUri + id.ToString()).Result;
            response.EnsureSuccessStatusCode();

            string contentType = response.Content.Headers.ContentType.MediaType;
            Debug.Assert(contentType == "application/json");
            Person person = response.Content.ReadAsAsync<Person>().Result;
            Console.WriteLine("{0} {1}", person.Name, person.Age);
        }

        private static void UseCopyToAsync(int id)
        {
            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync
                (_greetingsBaseUri + id.ToString()).Result;
            response.EnsureSuccessStatusCode();

            using (var stream = new FileStream(@"..\..\values.json", FileMode.Create))
            {
                response.Content.CopyToAsync(stream).Wait();
            }
        }

        private static void UseReadAsStream(int id)
        {
            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync
                (_greetingsBaseUri + id.ToString()).Result;
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;
            using (var reader = new StreamReader(stream))
            {
                string value = reader.ReadToEnd();
                Console.WriteLine(value);
            }
        }

        private static void UseReadAsString(int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                _greetingsBaseUri + id.ToString());
            request.Headers.Add("Accept", "application/xml");

            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string value = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(value);
        }

        private static void UseStringContent()
        {
            var content = new StringContent("\"Hello\"");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var response = client.PostAsync(_greetingsBaseUri, content).Result;
            response.EnsureSuccessStatusCode();
        }

        private static void UsePushStreamContent()
        {
            XNamespace ns = "http://schemas.microsoft.com/2003/10/Serialization/";
            var xml = new XElement(ns + "string", "Hello");

            var content = new PushStreamContent((stream, cont, ctx) =>
            {
                using (var writer = XmlWriter.Create(stream,
                    new XmlWriterSettings { CloseOutput = true }))
                // Write xml to the request stream
                { xml.WriteTo(writer); }
            });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            var client = new HttpClient();
            var response = client.PostAsync(_greetingsBaseUri, content).Result;
            response.EnsureSuccessStatusCode();
        }

        private static void UseStreamContent()
        {
            var stream = new FileStream(@"..\..\values.json", FileMode.Open, FileAccess.Read);
            using (var content = new StreamContent(stream))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var client = new HttpClient();
                var response = client.PostAsync(_greetingsBaseUri, content).Result;
                response.EnsureSuccessStatusCode();
            }
        }

        static void UseObjectContent()
        {
            var person = new Person { Name = "John", Age = 30 };
            var content = new ObjectContent<Person>(person,
                new JsonMediaTypeFormatter());
            //var request = new HttpRequestMessage(HttpMethod.Post, _personsBaseUri);
            //request.Content = content;

            var client = new HttpClient();
            client.PostAsync(_personsBaseUri, content);
        }
    }
}
