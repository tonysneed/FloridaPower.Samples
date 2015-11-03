Self Hosting ReadMe

1. Create a new Console Project.

2. Install self-hosting package for ASP.NET Web API
   - Microsoft.AspNet.WebApi.SelfHost
   
3. Add code to Program.Main to create a new HttpSelfHostConfiguration
   - Supply a base address

	var config = new HttpSelfHostConfiguration("http://localhost:12345");

4. Configure a default route

    config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
    );

5. Create a new HttpSelfHostServer in a using block, passing the config
   - Call OpenAsync on the server, then Wait
   - Add a Console.ReadLine to keep the console open

    using (HttpSelfHostServer server = new HttpSelfHostServer(config))
    {
        server.OpenAsync().Wait();
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }

6. Execute the program by pressing Ctrl+F5
   - You will probably get an AddressAccessDenied exception
   - Open an admin command prompt to add a URL reservation for port 12345
     + Substitue DOMAIN\user with your own local user account

    netsh http add urlacl url=http://+:12345/ user=DOMAIN\user

7. Add a ValuesController class that extends ApiController
   - Add a Get method returning a string array
   - Return an array of string values

    public string[] Get()
    {
        return new[] { "Value1", "Value2", "Value3" };
    }

8. Press Ctrl+F5 to launch the app again
  - Using Chrome navigate to http://localhost:12345/api/values
  - You should see XML output
  - Add the following to Application_Start:
  
  config.Formatters.Remove(config.Formatters.XmlFormatter);
  
  - You should now see a Json response
  


  