using System;
using System.Diagnostics;
using System.Web;
using System.Web.Http;

namespace WebHosting
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Debug.Print("Application_Start");

            GlobalConfiguration.Configure(config =>
            {
                // Configure Web API attribute routing
                config.MapHttpAttributeRoutes();

                // Remove the xml formatter 
                config.Formatters.Remove(config.Formatters.XmlFormatter);
            });
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Debug.Print("Session_Start");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Debug.Print("Application_BeginRequest");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Debug.Print("Application_AuthenticateRequest");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Debug.Print("Application_Error");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Debug.Print("Session_End");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Debug.Print("Application_End");
        }
    }
}