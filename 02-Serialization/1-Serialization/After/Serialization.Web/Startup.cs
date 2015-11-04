using System.Net.Http.Formatting;
using System.Web.Http;
using AspnetWebApi2Helpers.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using WebApiContrib.Formatting;

namespace Serialization.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Configure Json.Net serializer to use camel case property names
            config.Formatters.JsonFormatter.SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Configure Json.Net to handle cyclical references
            //config.Formatters.JsonFormatter.SerializerSettings
            //    .PreserveReferencesHandling = PreserveReferencesHandling.All;

            // Configure the Xml formatter serializer to handle cyclical references
            // - Add a reference to System.Runtime.Serialization
            //var dcs = new DataContractSerializer(typeof(Person), null, int.MaxValue,
            //    false, /* preserveObjectReferences */ true, null);
            //config.Formatters.XmlFormatter.SetSerializer<Person>(dcs);

            // Configure both json and xml to handle cycles
            // - Install the AspNetWebApi2Helpers.Serialization package
            config.Formatters.JsonPreserveReferences();
            config.Formatters.XmlPreserveReferences();

            // Add Bson media type formatter
            var bson = new BsonMediaTypeFormatter();
            bson.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            config.Formatters.Add(bson);

            // Add Protobuf formatter
            // - Add [ProtoContract] and [ProtoMember] attributes to Person class
            config.Formatters.Add(new ProtoBufFormatter());

            app.UseWebApi(config);
            app.UseWelcomePage();
        }
    }
}
