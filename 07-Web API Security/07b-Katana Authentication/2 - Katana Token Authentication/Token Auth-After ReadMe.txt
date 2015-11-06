Katana Token Authentication Demo: 'After' ReadMe

This demonstrates how to perform token-based authentication with a Web API service
that accepts OAuth bearer tokens.

PART A: Require Authentication for Replying Party

The first step to secure the RP is to require authentication for the Claims controller.

1. Open the ClaimsController class in the Relying Party project
   - Place an [Authorize] attribute on the class
   - Press Ctrl+F5 to launch the RP
   - Lauching the client app will now result in a 401 Unauthorized response

2. Now configure the RP to accept OAuth Bearer tokens
   - In Startup.Configuration call app.UseOAuthBearerAuthentication
     > Pass a new OAuthBearerAuthenticationOptions

        public void Configuration(IAppBuilder app)
        {
            // Consume bearer tokens
            var options = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(options);

   - At this point the client will still receive an Unauthorized response

PART B: Configure Token Service

The Token Service will be used to validate user credentials and issue OAuth Bearer tokens.

1. Add a DemoAuthorizationServerProvider to the Providers folder
   - Derive from OAuthAuthorizationServerProvider
   - Insert a GetUserClaims helper method

    private IEnumerable<Claim> GetUserClaims(string username)
    {
        return new[]
        {
            new Claim("sub", username),
            new Claim(ClaimTypes.Name, username),
        };
    }

2. Add a Validator property of type Func<string, string, Task<bool>>
   - This is a delegate that accepts username and password, and returns boolean
   - Add a ctor that initializes the Validator property

    // Credential validator
    public Func<string, string, Task<bool>> Validator { get; private set; }

    public DemoAuthorizationServerProvider
        (Func<string, string, Task<bool>> validator)
    {
        Validator = validator;
    }


3. Override GrantResourceOwnerCredentials to validate credentials,
   create a new claims identity, and create a new AuthenticationTicket
   - Call Rejected on the context if the user fails validation
   - Create the auth ticket using the claims identity
   - Otherwise, call Validated on the context, passing the ticket

    public async override Task GrantResourceOwnerCredentials
        (OAuthGrantResourceOwnerCredentialsContext context)
    {
        // Validate credentials
        if (!await Validator(context.UserName, context.Password))
        {
            context.Rejected();
            return;
        }

        // Create identity
        var identity = new ClaimsIdentity(GetUserClaims(context.UserName),
            context.Options.AuthenticationType);

        // Create ticket
        var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
        context.Validated(ticket);
    }


4. Lastly override ValidateClientAuthentication
   - We're not going to concern ourselves with OAuth client authentication,
     which validates credentials for the client application itself

    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        // OAuth2 has notion of client authentication, which is not used here
        context.Validated();
        return Task.FromResult<object>(null);
    }

5. Edit the Startup class to add a Validator method with the same signature
   as the Validator property of the Auth Service Provider.

    private Task<bool> Validator(string username, string password)
    {
        // NOTE: You'll want to validate base using a credentials repository

        // Valid if username and password are the same
        return Task.FromResult(username == password);
    }

6. Update Startup.Configuration to call app.UseOAuthAuthorizationServer
   - Pass a new OAuthAuthorizationServerOptions
     > Set the Provider to a new DemoAuthorizationServerProvider
	 > Also specify token endpoint path and a token expiration timeout

    // Use oauth authz server to issue tokens
    app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
    {
        AllowInsecureHttp = false, // May set to true for dev
        TokenEndpointPath = new PathString("/token"),
        AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
        Provider = new DemoAuthorizationServerProvider(Validator)
    });

   - Add a Bearer HostAuthenticationFilter to config.Filters

    config.Filters.Add(new HostAuthenticationFilter("Bearer"));

7. Run the Token Service
   - Test using a client such as Fiddler or Advanced REST client

   POST: http://localhost:50799/token
   Content-Type header: application/x-www-form-urlencoded
   Body: grant_type=password&username=tony&password=tony

   - You should receive an access token, which you can copy to the clipboard
     > Copy just the string in quotes following access_token
     > Note that username and password just need to be the same

8. Now run the Relying Party
   - Use Fiddler or Advanced REST client to issue GET:
     http://localhost:50858/api/claims
   - Set the Authorization Header as follows:
     Bearer access_token_string
	 > Paste the token string you copied earlier
	 > You should receive back a 200 OK response with user claims

PART C: Create Client App

The purpose of the client application will be to request an access token
from the Token Service, supplying username and password as credentials.
Then authenticate to the Relying Party by passing the token in the Authz header
when requesting a resource from the Reply Party.

1. Uncomment the code in Program.Main which prompts for credentials.

PART D: Use SSL with IIS (Optional)

1. If not done already, configure the web sites in IIS to use SSL
   - Add a binding for web.local for https

2. Specify machine keys in web.config for both RP and TS web apps
   - In IIS, select web app, Machine Key
   - Uncheck both checkboxes for generating a unique key for each app
   - Copy the keys from one web config file to another

3. Update the client url addresses.

