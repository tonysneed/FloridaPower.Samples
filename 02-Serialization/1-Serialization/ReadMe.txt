Serialization ReadMe

Here we'll configure Json and Xml serializers, including cyclical reference handling.
We'll also enable bson and protobuf for binary encoding to improve performance.

Part A: Configure Json.NET Serializer

1. Press Ctrl+F5 to start the web app
   - Use Fiddler or another client to submit an HTTP request
     with an Accept header of application/json
     > Uri: http://localhost:57986/api/Persons/1
	 > Note the port must match that of the web project
   - Note that Json property names are title cased

2. Add code to the Configuration method of the Startup class to access Json formatter.

	config.Formatters.JsonFormatter.SerializerSettings

3. Specify camel-cased property names
   - Set ContractResolver to a new CamelCasePropertyNamesContractResolver

    config.Formatters.JsonFormatter.SerializerSettings
        .ContractResolver = new CamelCasePropertyNamesContractResolver();

   - Build the app and refresh the browser
     > Note that property names are now camel cased

Part B: Handle cyclical references

1. Uncomment line 9 of the Person class to add a Brother property
   - Then uncomment code in PersonsController ctor which sets the Brother property
   - Build the app and refresh the browser
   - Note the serialzation error due to cycles
   - Perform the same test using Fiddler to see the error for the Json serializer

2. We'll start by fixing the problem for Json
   - Adding this attribute to the Person class will resolve the error:
     [JsonObject(IsReference=true)]
	 > But this will pollute the entity with serialization concerns, which is not ideal
   - Configure the serializer to preserve references

    config.Formatters.JsonFormatter.SerializerSettings
        .PreserveReferencesHandling = PreserveReferencesHandling.All;

   - Rebuild the app and re-issue the request using Fiddler
     > The error will go away and the Json will include object references

3. Next we'll configure the Xml formatter to handle cycles
   - First add a refernce to System.Runtime.Serialization
   - Create a new DataContractSerializer with a ctor overload in which you can
     set the preserveObjectReferences parameter to true
   - Then call SetSerializer on the XmlFormatter, passing the dcs

    var dcs = new DataContractSerializer(typeof(Person), null, int.MaxValue,
        false, /* preserveObjectReferences */ true, null);
    config.Formatters.XmlFormatter.SetSerializer<Person>(dcs);

	- Build and refresh the web page to resolve the serialization error
	- Notice that you need to do this on a per-type basis, which is not ideal
	- To make this easier, you can install the AspNetWebApi2Helpers.Serialization
	  package and configure the serializers at the Formatters level

    config.Formatters.JsonPreserveReferences();
    config.Formatters.XmlPreserveReferences();

Part C: Add Bson and Protobuf Formatters

1. Add a new BsonMediaTypeFormatter to config.Formatters
	- Be sure to config the serializer to preserve references

    var bson = new BsonMediaTypeFormatter();
    bson.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
    config.Formatters.Add(bson);

	- Build the app and execute a request from Fiddler,
	  specifying the Accept header as application/bson
	  > If you compare the body size of Json to Bson you'll see Bson is larger

2. To get a smaller payload we can add the Protobuf formatter
   - Add the NuGet package WebApiContrib.Formatting.ProtoBuf
   - Add [ProtoContract] to the Person class
   - Add [ProtoMember] to each property
     > For Brother specify AsReference=true

     > Specify the assembly for the Person type

    config.Formatters.Add(new ProtoBufFormatter());

	- Build the app and submit a request using the Accept Header: application/x-protobuf
	- Note the smaller message body size

