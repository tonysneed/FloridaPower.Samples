Demo: ASP.NET 5 Configuration and Dependency Injection

In this demo we'll experiment with configuration.

1. Add a config.json file to the project
   - Add a Greeting section with different greetings

  "Greeting": {
    "DefaultGreeting": "Hello",
    "WesternGreeting": "Howdy",
    "EasternGreeting" : "Yo"
  },

2. Add the following dependency to the project.json file:

   Microsoft.Framework.ConfigurationModel.Json
   
3. Add a Greeting class to the Properties folder

    public class Greeting
    {
        public string DefaultGreeting { get; set; }
        public string WesternGreeting { get; set; }
        public string EasternGreeting { get; set; }
    }

4. Add code to Startup to set up configuration
   - This will allow you to inject IOptions<Greeting> into controllers

    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new Configuration()
                .AddJsonFile("config.json");
        }

        public void ConfigureServices(IServiceCollection services)
        {
		    // Config service
            services.Configure<Greetings>(Configuration.GetSubKey("Greetings"));	
        
5. Add ctor to ValuesController to inject config options and environment
   - This will inject both greetings options and hosting environment

    public class ValuesController : Controller
    {
        IHostingEnvironment _env;
        IOptions<Greeting> _greetingConfig;

        public ValuesController(IHostingEnvironment env,
            IOptions<Greeting> greetingConfig)
        {
            _env = env;
            _greetingConfig = greetingConfig;
        }

6. Use the environment and options in Get action
   - This will get the default greeting,
   - But it will get the western greeting if environment is Development

    public string Get(int id)
    {
        var greeting = _greetingConfig.Options.DefaultGreeting;
        if (_env.IsEnvironment("Development"))
        {
            greeting = _greetingConfig.Options.WesternGreeting;
        }
        return greeting;
    }

   - Running the app in IIS Express will pick up the western greeting,
     because launchSettings will set the environment to Development.
