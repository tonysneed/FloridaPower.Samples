Katana Basic Auth Demo ReadMe

This demonstrates how to perform basic authentication with Katana middleware.

NOTE: Here are steps taken to set up IIS hosting with OWIN.
      You should have already set up SSL on IIS with web.local cert.
	  You will also need to open Visual Studio as an Administrator.
	  Configure Fiddler to decrupt HTTPS traffic.

SETUP: OWIN IIS Hosting with SSL

1. Create an Emtpy Web project and add the following NuGet packages:
   - Microsoft.Owin.Host.SystemWeb
   - Microsoft.AspNet.WebApi.Owin
   - Microsoft.Owin.Security

2. Add a Startup class to the project.
   - Use VS item template, which adds the Startup attribute

    public void Configuration(IAppBuilder app)
    {
		// Configure web api routing
		var config = new HttpConfiguration();
		config.MapHttpAttributeRoutes();
		config.Routes.MapHttpRoute(
			"DefaultApi", 
			"api/{controller}/{id}",
			new { id = RouteParameter.Optional });
		app.UseWebApi(config);

		// Add error, welcome pages
		app.UseErrorPage();
		app.UseWelcomePage();
    }

3. Set web server to Local IIS
   - Open project properties, Web tab
   - Select Local IIS for the web server
   - Specify the url:
     https://web.local/KatanaBasicAuth
   - Press Ctrl+F5 to see if the setup is OK
     > Issue a GET request to the Hello controller
     https://web.local/KatanaBasicAuth/api/hello

4. Test the client and inspect traffic using Fiddler
   - Run the client, both with and without credentials
   - Notice there is no auth challenge from the server
     and that the client in unauthenticated.

Part A: Implement Basic Auth using Katana

NOTE: Basic Auth Katana Middleware has been implemented by classes
      in the Authentication folder:

	  BasicAuthenticationOptions
	  BasicAuthenticationHandler
	  BasicAuthenticationMIddleware
	  BasicAuthenticationExtensions

1. Complete the TODO in web app Startup to use basic auth
   - This will use the supplied method to validate credentials

    app.UseBasicAuthentication("KatanaBasicAuth", Validator);

2. Place an [Authorize] attribute on the ClaimsController
   - This will cause the basic auth handler to issue a response challenge
     requiring authentication

3. Test the client both without and with credentials,
   as well as invalid credentials
   - Inspect the traffic using Fiddler
   - Notice the return of the WWW-Authenticate header with the 401 response

