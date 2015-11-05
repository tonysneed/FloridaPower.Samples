Model Binding ReadMe

This sample demonstrates how to use model binding with Web API.

Part A: Model Binding Attributes

1. Start up Fidder or another REST client
   - For example, Google Chrome Extension: Advanced REST Client
   - Start the web app and send the following request:
     > Uri: http://localhost:57988/api/persons/4
	 > Verb: POST
	 > Body: Name=Joe&Age=25
   - Submit a GET to the same Uri to verify that the Person was added

2. Let's say we wanted to submit the name and age via the Uri
   - Default model binding maps simple parameter types to uri segments,
     but complex types are mapped to the message body and serialized
	 using a media type formatter.
   - Add a [FromUri] attribute to the Person parameter of Post
   - Then resubmit the POST request with name and age in the Uri
     > Uri: http://localhost:57988/api/persons/5?Name=John&Age=35
	 > Verb: POST
	 > Body: None
   - Submit a GET to the same Uri to verify that the Person was added

Part B: Custom Type Converter

1. Submit GET and POST requests for the Locations controller
   - GET: http://localhost:57988/api/locations
   - POST: http://localhost:57988/api/locations/1,2
     > POST should result in a 0,0 location added, which is incorrect

2. There is a LocationTypeConverter class in the Models folder
   which converts a string (1,2) into a Location
   - Add a TypeConverter attribute to the Location class and
     specify LocationTypeConverter
   - Remove the [FromUri] attribute from before the value parameter in Post
   - Resubmit the POST request to verify that location 1,2 was added

3. For more complex conversion logic that can be encapsulated, create a model binder
   - Have a look at LocationModelBinder in the Models folder, which looks up
     location from a data cache based on a key
   - Comment out the TypeConverter attribute on the Location class
   - Add a ModelBinder attribute, specifying the LocationModelBinder
   - Resubmit the POST request to verify that location 1,2 was added

4.  Resubmit the request specifying a cache key (top-right)
    instead of a location (1,2)
   - Resubmit the POST request to verify that location 100,100 was added

