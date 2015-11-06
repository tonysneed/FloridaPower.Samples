using System;
using Microsoft.Owin.Hosting;

namespace OwinSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("https://web.local:54321/"))
            {
                Console.WriteLine("Web app started ...");
                Console.ReadLine();
            }
        }
    }
}
