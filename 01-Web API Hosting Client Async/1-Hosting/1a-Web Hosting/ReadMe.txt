Web Hosting ReadMe

1. Create a new Web Project.
   - Select the Empty template

2. Install references and packages for ASP.NET Web API
   - Add a reference to System.Net.Http 4.0
   - Add the following NuGet package: Microsoft.AspNet.WebApi
   
3. Add a new item, selecting Global Application Class
   - A Global.asax file is added with a code behind class that
     has a number of methods which hook into the ASP.NET pipeline.
   - Insert Debug.Print statements in each method to write the
     method name to the output window.
   - Press F5 to debug the app, notice the following output:
     Application_Start
     Application_BeginRequest
     Application_AuthenticateRequest

     
4. Add code Application_Start to configure Web API routing
   - We'll just use attribute based routing

  GlobalConfiguration.Configure(config =>
  {
      config.MapHttpAttributeRoutes();
  });
  
5. Add a ValuesController class that extends ApiController
   - Add a Get method returning a string array
   - Add a Route attribute with an "api/values" parameter
   - Return an array of string values

    [Route("api/values")]
    public string[] Get()
    {
        return new[] {"Value1", "Value2", "Value3"};
    }

6. Press Ctrl+F5 to launch the site with IIS Express
  - Using Chrome navigate to http://localhost:{port}/api/values
  - You should see XML output
  - Add the following to Application_Start:
  
  config.Formatters.Remove(config.Formatters.XmlFormatter);
  
  - You should now see a Json response
  


  