Owin Hosting ReadMe

Part A: Web-Hosting with Owin

1. Create an empty Web Project.
   - Add the NuGet package: Microsoft.Owin.Host.SystemWeb

2. Add a GreetingComponent class
   - Add a _next field of type Func<IDictionary<string, object>, Task> 
   - Add a ctor that initializes the field
   - Add an Invoke method that returns a Task and accepts
     an IDictionary<string, object> environment parameter
   - Note that while we're not using _next, we need the ctor or
     we'll get a MissingMethod exception

    public class GreetingComponent
    {
        private Func<IDictionary<string, object>, Task> _next;

        public GreetingComponent(Func<IDictionary<string, object>, Task> next)
        {
            // Get pointer to next component in the pipeline
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            // TODO: Get response stream and write to it
        }
    }

   - In the body of the Invoke method, write to the response stream

    var response = (Stream)environment["owin.ResponseBody"];
    using (var writer = new StreamWriter(response))
        return writer.WriteAsync("Hello from Owin!");

3. Add a new item to the project, selecting "OWIN Startup Class"
   - Give it the name Startup.cs
   - Nameing the class Startup eliminates the need for the assembly attribute
   - Add code to the Configuration method which uses the Greeting component

   app.Use<GreetingComponent>();

   - Press Ctrl+F5 to launch the web app using IIS Express
   - You should see the output: Hello from Owin!

4. Add a class called LoggingComponent
   - Add a _next field as in the GreetingComponent
   - Initialize the field from the ctor
   - Add an Invoke method as before
   - Debug.Print owin.RequestPath environment variable
   - Debug.Print owin.ResponseStatusCode environment variable
   - Return a blank task

    public Task Invoke(IDictionary<string, object> environment)
    {
        // Log request and response info
        Debug.Print("Request Path: {0}", environment["owin.RequestPath"]);

        // Log response
        Debug.Print("Response Status Code: {0}", environment["owin.ResponseStatusCode"]);
            
        // Return blank task
        return Task.FromResult<object>(null);
    }

5. Add the logging component to the Startup.Configuration method
   - Insert app.Use for logging prior to greeting
   - Press F5 to debug the app and view messages in the output window
   - Notice the following printed to the Debug window:
     Request Path: /
	 Response Status Code: 200
   - However, the Greeting component is never invoked because the Logging
     component did not invoke the next delegate in the chain

6. To correct this, refactor Invoke in Logging component to call _next(environment)
   - Place the call after printing the request path, but before the response code
   - To preserve the async nature of the call, await the call to _next,
     and mark the Invoke method as async
	 > You can then remove the line of code that returns a blank task
   - This time notice that the greeting component is invoked

    public async Task Invoke(IDictionary<string, object> environment)
    {
        // Log request and response info
        Debug.Print("Request Path: {0}", environment["owin.RequestPath"]);

        // Invoke next component in the chain of middleware
        await _next(environment);

        // Log response
        Debug.Print("Response Status Code: {0}", environment["owin.ResponseStatusCode"]);
            
        // Return blank task
        //return Task.FromResult<object>(null);
    }

Part B: Self-Hosting with Owin

1. Add a Console project to the solution
   - Add the NuGet packages:
     Microsoft.Owin.SelfHost
     Microsoft.AspNet.WebApi.OwinSelfHost

2. Add a LoggingComponent class that extends the OwinMiddleware abstract class
   - Implement the members: ctor, Invoke
   - Notice that Invoke takes an IOwinContext, which provides strongly typed
     access to the environment dictionary
     > So refactor the Debug.Pring methods to use the context
   - To invoke the next middleware component, call Next.Invoke(context)

3. Add a Startup class to the project
   - This time, just add a regular class without using the OWIN template
   - Add a Configuration method which brings in two components:
     app.Use<LoggingComponent>
	 app.UseWelcomePage
   
4. Insert code in Program.Main to start an HTTP listener
   - Note that there is no need for a URL reservation to be made via netsh
   - However, if the url of the app matches an existing reservation,
     an Access denied error will take place.

    using (WebApp.Start<Startup>("http://localhost:8080"))
    {
        Console.WriteLine("Press Enter to exit");
        Console.ReadLine();
    }

5. Press Ctrl+F5 to start the console app, then open a browser to the address in Main
   - You should see the nice welcome page in the browser
   - You should also see output to the console window from the loggin component

6. Add a line to Startup.Configuration for displaying an error page
   - Place this code before the other app.Use statements:
     app.UseErrorPage
   - Add an app.Use statement just before the UseWelcomePage which throws an
     exception for a url ending in /fail

	app.Use(async (context, next) =>
    {
        if (context.Request.Path.ToString() == "/fail")
            throw new Exception("Doh!");
        await next();
    });

	- Now start the app and append the url with /fail to see the error page

7. Next, add middleware for Web API
   - Place the following in Starup.Configuration, just after the logging component

    var config = new HttpConfiguration();
    config.MapHttpAttributeRoutes();
    config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}",
        new { id = RouteParameter.Optional });
	app.UseWebApi(config);

8. Add a ValuesController class that extends ApiController
   - Add a Get method returning an array of string values

    // api/values
    public string[] Get()
    {
        return new[] { "Value1", "Value2", "Value3" };
    }

	- Navigate to http://localhost:8080/api/values
	  > You should see XMl or JSON response
	  > Feel free to try it with Fiddler or another client,
	    setting the Accept header to application/json

Part C: Hosting with OwinHost

1. Add an Empty web project to the solution
   - Add the following NuGet packages:
     OwinHost
     Microsoft.AspNet.WebApi.Owin

2. Add a Startup class as before
   - Add code for configuring and using Web API

3. Add a ValuesController class as before
   - Include the Get method

4. Open the project Properties page and select the Web tab
   - Under Servers, select OwinHost
   - Change the port to an address without a URL reservation, such as 8888

5. Running the app will generate a runtime exception
   - To resolve the error, place the following in web.config

  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" culture="neutral"/>
        <bindingRedirect oldVersion="0.0.0.0-3.0.0.0" newVersion="3.0.0.0"/>
      </dependentAssembly>
    </assemblyBinding>
  </runtime>

6. Run the app again and navigate to a the values controller
   - http://localhost:8888/api/values
   - You should need the expected response

