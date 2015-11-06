OWIN Self Host SSL Demo ReadMe

This demonstrates how to configure an OWIN self-hosted app
to use SSL for transport security. A console app is used for
demo purposes, but this could also be a Windows Service.

PART A: OWIN Self Hosting

1. Create a console app, add the following NuGet packages:
   - Microsoft.Owin.SelfHost
   - Microsoft.AspNet.WebApi.OwinSelfHost

2. Add a controller
    public class HelloController : ApiController
    {
        // GET: api/hello
        public IHttpActionResult Get()
        {
            return Ok("Hello Web API Self Host");
        }
    }

3. Add a Startup class to the project.
   - Add the following method:

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

		// Add welcome page
		app.UseWelcomePage();
    }

4. Add the following to Program.Main:

	using (WebApp.Start<Startup>("http://localhost:54321/"))
	{
        Console.WriteLine("Web app started ...");
		Console.ReadLine();
	}

5. Run the console app, then open a browser
   - Navigate to: http://localhost:54321/
     > You should see the welcome page.
   - Navigate to: http://localhost:54321/api/hello
     > You should see a json or xml response

Part B: Set up SSL

NOTE: First create a Certificates MMC link.
      Run, mmc, Add/Remove snap-in, Certificates, Computer Account, Local Computer.
	  Save the snap-in as Certificates.

1. Create a folder to contain certs, then open a command-prompt there.
   - Set path to tools folder in the repo root
   - Execute: CreateDevRoot
     > Click None when prompted for a password
   - Execute: CreateSslCert web.local

2. Install the certificates
   - Double click DevRoot.pfx
     > Select Local Machine
	 > Place in Trusted Root Certification Authorities
	 > Verify that the DevRoot certificate was installed
   - Double click web.local.pfx to import it to LocalMachine
     > Select the defaults
	 > Verify web.local was added to Personal Certificates in Local Machine

3. Map loop back address to web.local
   - Open Notepad as admin, go to %windir%\System32\drivers\etc
     and open hosts file
   - Add following line:
   	 127.0.0.1       web.local
	 > Save and close the file

4. Change the host name in Program.Main from localhost to web.local
   - Running the app will generate an access denied exception
   - Open an admin command prompt and execute:
     netsh http show urlacl
   - Navigate to the tools folder and execute HttpConfig
	 > View existing URL reservations
	 > Add: http://web.local:54321/ with your local user account
	 > Click OK
   - Press Ctrl+F5 to run the OWIN self host app again
     > Verify that no security exception has been thrown

5. Open HttpConfig again to add a certificate binding
   - On the SSL tab click Add
   - Enter IP: 0.0.0.0, Port: 54321, Cert: Select web.local, Guid: New
   - Change the url in the app to use https
   - If you run the app again, you'll get a different error - reservation conflict
     > Remove the existing reservation
	 > Add: https://web.local:54321/ with your local user account
   - Run app and open a browser to this location to verify that it works

6. Set up IIS SSL bindings (optional)
   - Open IIS Manager, Default Web Site, Bindings
   - Add a binding for port 443
     > IP Addresses: All Unassigned
	 > Host name: web.local (also add one with a blank name)
	 > SSL Certificate: web.local
   - Browse the unnamed 443 binding, note ssl error
   - Browse web.local on 443, note green lock

