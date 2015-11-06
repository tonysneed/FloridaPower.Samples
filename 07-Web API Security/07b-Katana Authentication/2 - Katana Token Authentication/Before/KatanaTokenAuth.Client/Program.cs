using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KatanaTokenAuth.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create http client
            const string address = "https://web.local:4444/api/";
            var client = new HttpClient { BaseAddress = new Uri(address) };

            // Prompt for credentials
            //Console.WriteLine("Authenticate? {Y/N}:");
            //bool authenticate = Console.ReadLine().ToUpper() == "Y";
            //if (authenticate)
            //{
            //    Console.WriteLine("Enter user name:");
            //    string username = Console.ReadLine();
            //    Console.WriteLine("Enter password:");
            //    string password = Console.ReadLine();
            //    string token = GetToken(username, password);
            //    client.DefaultRequestHeaders.Authorization =
            //        new AuthenticationHeaderValue("Bearer", token);
            //}

            // Issue get request
            var response = client.GetAsync("claims").Result;
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
            if (claims != null && claims.Any())
            {
                Console.WriteLine("\nUser claims:");
                foreach (var claim in claims)
                {
                    Console.WriteLine("{0} {1}", claim.Type, claim.Value);
                }
            }
            else
            {
                Console.WriteLine("User not authenticated.");
            }

            Console.WriteLine("\nPress Enter to exit");
            Console.ReadLine();
        }

        private static string GetToken(string username, string password)
        {
            const string address = "https://web.local:5555/";
            var client = new HttpClient { BaseAddress = new Uri(address) };

            var fields = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"username", username},
                {"password", password},
            };

            var response = client.PostAsync("token", new FormUrlEncodedContent(fields)).Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            var tokenResponse = new TokenResponse(content);
            return tokenResponse.AccessToken;
        }
    }
}
