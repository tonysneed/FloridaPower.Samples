using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace KatanaBasicAuth.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create http client
            const string address = "https://web.local/KatanaBasicAuth-After/api/claims";
            var client = new HttpClient { BaseAddress = new Uri(address) };

            // Prompt for credentials
            Console.WriteLine("Authenticate? {Y/N}:");
            bool authenticate = Console.ReadLine().ToUpper() == "Y";
            if (authenticate)
            {
                Console.WriteLine("Enter user name:");
                string username = Console.ReadLine();
                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();
                string token = EncodeCredentials(username, password);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", token);
            }

            // Issue get request
            var response = client.GetAsync("").Result;
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException requestEx)
            {
                Console.WriteLine(requestEx.Message);
                return;
            }
            var claims = response.Content.ReadAsAsync<IEnumerable<ClaimInfo>>().Result;

            Console.WriteLine("\nUser claims:");
            foreach (var claim in claims)
            {
                Console.WriteLine("{0} {1}", claim.Type, claim.Value);
            }

            Console.WriteLine("\nPress Enter to exit");
            Console.ReadLine();
        }

        private static string EncodeCredentials(string username, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}",
                new object[] { username, password })));
        }
    }
}
