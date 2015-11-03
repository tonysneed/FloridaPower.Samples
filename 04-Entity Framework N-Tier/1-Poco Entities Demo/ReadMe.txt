POCO Entities Demo

NOTE: Install SQL Express or LocalDb
      
      Create NorthwindSlim database, run script

Part A: Use EF with Web API

1. Open the Before solution, which only contains a
   Web API project.
   - Includes a Web API Help page with a test client
   
2. Create the NorthwindSlim database
   - View Server Explorer, right-click Data Connections
   - Select Create New Database
     > (localdb)\v11.0
     > NorthwindSlim
   - Download db script: http://bit.ly/northwindslim
   - Open NorthwindSlim.sql in VS, Connect to (LocalDb)\v11.0
   - Execute the NorthwindSlim.sql script
      
3. Add a new Class Library project to the solution
   - Name it PocoDemo.Data
   - Add an ADO.NET Entity Data Model
     > Name it NorthwindSlim
     > Select Code First from Database
     > Select NorthwindSlim data connection
     > Select Category and Product tables
     
4. Add EF NuGet package to the Web project
   - Copy the connection strings section from the Data project
     to web.config in Web project
   - Reference the Data project from the Web project
     
5. Add a ProductsController to the Web project
   - Right-click the Controllers folder,
     select Add Controller
   - Select Web API 2 Controller - Empty
   
6. Add Get action to retrieve all products,
   sorted by ProductName
   - Include ResponseType attribute
   - Return Task<IHttpActionResult> with async
   - Await call to ToListAsync
   
	[ResponseType(typeof(IEnumerable<Product>))]
	public async Task<IHttpActionResult> Get()
	{
		using (var dbContext = new NorthwindSlim())
		{
			var products = await dbContext.Products
				.OrderBy(p => p.ProductName)
				.ToListAsync();
			return Ok(products);
		}
	}

7. View Web app in browser
   - Test api/Products
   - Notice the error due to dynamic proxies
   - Turn off proxy creation in ctor of NorthwindSlim DbContext class
     > Configuration.ProxyCreationEnabled = false;
     
Part B: Customize T4 Code Gen Templates

NOTE: Install the Tangible T4 Editor (Tools, Extensions)

1. Add EF T4 Templates to the Data project
   - Add EntityFramework.CodeTemplates.CSharp NuGet package
   
2. Modify DbContext T4 template
   - Open Context.cs.t4 template and add code to the DbContext
     ctor to disable proxy generation
   - Locate the OnModelCreating method and add the following:
     modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
   - Add the following using directive:
     using System.Data.Entity.ModelConfiguration.Conventions;
   - Instruct Code First to use an existing database
     > Insert the following code in the NorthwindSlim ctor:
	   Database.SetInitializer<NorthwindSlim>(null);
   
3. Modify the EntityType T4 template
   - Remove two using directives with DataAnnotations
   - Remove using for System.Data.Entity.Spatial
   - Remove the virtual qualifier from the navigation property
   - Remove code that adds the type config attributes
   - Remove code that adds property config attributes
   
4. Remove classes and cn string, then re-add the Data Model
   - Notice that the DbContext and entity classes reflect
     changes to the T4 templates

Part C: Separating Entities from DbContext

1. Add a Portable Class Library project to the solution
   - Name it PocoDemo.Entities
   - Set targets: .NET 4, Win 8, Win Phone SL 8, SL5, Win Phone 8
   - Remove Class1.cs from the project
   
2. Exclude Category and Product classes from the Data project
   - Reference the Entities project from the Data project
   - Reference the Entities project from the Web project
   
3. Build the solution and test the Products controller
   - Go to the api/Products help page and click Test API button

Part D: Handle Cyclical References

1. Modify the Get action of the Products controller
   to include the Category property
   - Test the Products controller again
   - You should receive an exception stating:
     Self referencing loop detected
     
2. Use Attributes to resolve cyclical references
   - Add the Newtonsoft.Json NuGet package to the Entities 
   - Add the following attribute to Category, Products classes:
   [JsonObject(IsReference = true)]
   - Retest the Products controller
     > There should be no exception this time
     
3. Use code to resolve Json cyclical references
   - Remove the Json.Net package and the attributes
     from the Entities project
   - Insert the following code in WebApiConfig.Register:
	 config.Formatters.JsonFormatter.SerializerSettings
		.PreserveReferencesHandling = PreserveReferencesHandling.All;
   - Re-test the Products controller

4. Add a helper library for configuring serializers
   - AspNetWebApi2Helpers.Serialization -Pre
     > Select Include Prerelease if using the NuGet UI
   - Replace the above code to use the helper library
     config.Formatters.JsonPreserveReferences();
   
5. Re-test the Products controller by adding an Accept header
   for application/xml
   - The Data Contract Serializer will complain about cycles
   - Add the following code to WebApiConfig:
     config.Formatters.XmlPreserveReferences();

Part E: Binary wire format

Note: Download and install Fiddler4:
http://www.telerik.com/download/fiddler

1. Test the products controller with fiddler running
   - note the size of the json body payload
   
2. Configure a protobuf formatter to handle cycles:
   - Add NuGet package: AspNetWebApi2Helpers.Serialization.Protobuf -pre
   - Configure the protobuf formatter as follows:
   
	var protoFormatter = new ProtoBufFormatter();
	protoFormatter.ProtobufPreserveReferences(typeof(Category)
		.Assembly.GetTypes());
	config.Formatters.Add(protoFormatter);
	
Part F: Client
   
1. Add a new console project: PocoDemo.Client
   - Add the NuGet packages:
     AspNetWebApi2Helpers.Serialization -pre
	 AspNetWebApi2Helpers.Serialization.Protobuf -pre
   - Reference the Entities project
   
2. Add the following code:

	var client = new HttpClient { BaseAddress = new Uri("http://localhost:51246/api/products/") };
	HttpResponseMessage response = client.GetAsync("").Result;
	response.EnsureSuccessStatusCode();
	var products = response.Content.ReadAsAsync<List<Product>>().Result;
	foreach (var p in products)
	{
		Console.WriteLine("{0} {1} {2} {3}",
			p.ProductId,
			p.ProductName,
			p.UnitPrice.GetValueOrDefault().ToString("C"),
			p.Category.CategoryName);
	}

   - Run the browser client to test it
   
3. Prompt the user for a media type
   - Set the media type formatter and accept header value

	// Prompt user for media type
	Console.WriteLine("Select media type: {1} Xml, {2} Json, {3} Bson, {4} Protobuf");
	int selection = int.Parse(Console.ReadLine());

	// Configure accept header and media type formatter
	MediaTypeFormatter formatter;
	string acceptHeader;
	switch (selection)
	{
        case 1:
            formatter = new XmlMediaTypeFormatter();
            ((XmlMediaTypeFormatter)formatter).XmlPreserveReferences
                (typeof(Category), typeof(List<Product>));
            acceptHeader = "application/xml";
            break;
        case 2:
            formatter = new JsonMediaTypeFormatter();
            ((JsonMediaTypeFormatter)formatter).JsonPreserveReferences();
            acceptHeader = "application/json";
            break;
        case 3:
            formatter = new ProtoBufFormatter();
            ((ProtoBufFormatter)formatter).ProtobufPreserveReferences
                (typeof(Category).Assembly.GetTypes());
            acceptHeader = "application/x-protobuf";
            break;
		default:
			Console.WriteLine("Invalid selection: {0}", selection);
			return;
	}

   - Create an http client with service base address
   
	var client = new HttpClient
	{
		BaseAddress = new Uri("http://localhost:51246/api/products/"),
	};
	
   - Set request accept header
   
   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader));
      
   - Read response content as string array
        
	var products = response.Content.ReadAsAsync<List<Product>>
		(new[] { formatter }).Result;
	foreach (var p in products)
	{
		Console.WriteLine("{0} {1} {2} {3}",
			p.ProductId,
			p.ProductName,
			p.UnitPrice.GetValueOrDefault().ToString("C"),
			p.Category.CategoryName);
	}
   
   - Append .fiddler to localhost to see the traffic in Fiddler
     > Compare payload sizes

