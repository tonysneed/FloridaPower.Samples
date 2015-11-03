Demo: ASP.NET 5 using Visual Studio

Part A: Empty Web App

In this demo we'll create an empty ASP.NET 4 web app using Visual Studio 2015.

1. Create a new ASP.NET Web App using Visual Studio 2015
   - Select the empty ASP.NET 5 template
   - Inspect project.json and startup.cs
   
2. Try running the app using both IIS and web profiles
   - For web: browse to http://localhost:5000

3. Insert middleware that prints the request path and response status code
   - Place the following code in the Startup.Configure method

    app.Use(next => async context =>
    {
        Console.WriteLine("Request Path: {0}", context.Request.Path);
        await next.Invoke(context);
        Console.WriteLine("Response Status Code: {0}", context.Response.StatusCode);
    });

Part B: Web API App

1. Create a new web project using Visual Studio 2015
   - Select the ASP.NET 5 Web API template
   - Inspect project.json and startup.cs
   - Inspect launchSettings.json in Properties folder

2. Run the app using both IIS and web profiles
   - For web profile, browse to: http://localhost:5000/api/values
   - Get should return JSON

