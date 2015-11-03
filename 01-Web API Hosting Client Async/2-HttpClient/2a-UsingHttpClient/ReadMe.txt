Using HttpClient ReadMe

Before solution is uses Web API with Owin Web hosting. However, this demo
is for using HttpClient, so the web service does not need Owin.  The client
project includes the NuGet package: Microsoft.AspNet.WebApi.Client.

1. Restore NuGet packages for the solution

2. Start the OwinWebHosting project by pressing Ctrl+F5
   - Add /api/greetings to the URL to retrieve greetings

3. Add a method to Program.Main in UsingHttpClient to retrieve Json string of greetings
   - Use async and await to return a Task<string>
   - Create a new HttpClient, setting the BaseAddress to _greetingsBaseUri
   - Call GetStringAsync passing an empty string
   - Call the method from Main and print Result property to the console
     > Note this will block the main thread until the result is returned

4. Try changing the base address to add ".fiddler" after localhost, then
   sniff the traffic using Fiddler
   - Inspect the request and response using Fiddler
   - Free free to remove .fiddler from the request URL then quit Fiddler

5. Write a method to get a greeting by id as a JSON string
   - Call client.GetStringAsync, passing id.ToString()

6. Write a method to get a greeting by an invalid id
   - Using a Fiddler, we can see that we get back a 404 Not Found
   - But GetStringAsync just gives us a 500 error
   - Refactor the method to call client.GetAsync, returning HttpResponseMessage

