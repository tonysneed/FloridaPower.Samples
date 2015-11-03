using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PocoDemo.Data;
using WebApiContrib.Formatting;
using AspnetWebApi2Helpers.Serialization;
using AspnetWebApi2Helpers.Serialization.Protobuf;
using FluentValidation.WebApi;
using PocoDemo.Web.Validation;

namespace PocoDemo.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configure Json and Xml formatters
            config.Formatters.JsonPreserveReferences();
            config.Formatters.XmlPreserveReferences();

            // Configure ProtoBuf formatter
            var protoFormatter = new ProtoBufFormatter();
            protoFormatter.ProtobufPreserveReferences(typeof(Category)
                .Assembly.GetTypes());
            config.Formatters.Add(protoFormatter);

            // Add validation filter
            config.Filters.Add(new ValidateModelAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
