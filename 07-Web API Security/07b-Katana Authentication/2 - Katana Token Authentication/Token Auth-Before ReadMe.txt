Katana Token Authentication Demo: 'Before' ReadMe

This demonstrates how to perform token-based authentication with a Web API service
that accepts OAuth bearer tokens. This ReadMe contains instructions for setting up
the 'Before' solution.

PART A: Create Replying Party

The Relying Party is a web app requiring authentication for protected resources.
In this example, the RP has a Web API service with a ClaimsController class that
returns claims for an authenticated user.

1. Create an emtpy Web project for the Relying Party.
   Add the following NuGet packages:
   - Microsoft.Owin.Host.SystemWeb
   - Microsoft.AspNet.WebApi.Owin
   - Microsoft.Owin.Security.OAuth
   - Microsoft.Owin.Diagnostics
   - OwinHost (Reponse 'Yes' to the prompt for VS 2013 compatibility)

2. Add a Startup class to the RP app
   - Add web api, error and welcome pages

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

PART B: Create Token Service

The Token Service will be used to validate user credentials and issue OAuth Bearer tokens.

1. Create an Emtpy Web project for the Token Service. 
   Add the following NuGet packages:
   - Microsoft.Owin.Host.SystemWeb
   - Microsoft.AspNet.WebApi.Owin
   - Microsoft.Owin.Security.OAuth
   - Microsoft.Owin.Diagnostics
   - OwinHost (Reponse 'Yes' to the prompt for VS 2013 compatibility)

2. Add a Startup class to the RP app
   - Add error and welcome pages

    public void Configuration(IAppBuilder app)
    {
        // Add error, welcome pages
        app.UseErrorPage();
        app.UseWelcomePage();
    }

PART C: Create Client App

The purpose of the client application will be to request an access token
from the Token Service, supplying username and password as credentials.
Then authenticate to the Relying Party by passing the token in the Authz header
when requesting a resource from the Reply Party.

1. Create an emtpy Web project for the Relying Party.
   Add the following NuGet packages:
   - Microsoft.AspNet.WebApi.Client
   - Newtonsoft.Json

2. First lauch the Relying Party web app by pressing Ctrl+F5
   - Then launch the Client app to verify it can invoke the web service
     > A message should be displayed that the user is not authenticated

