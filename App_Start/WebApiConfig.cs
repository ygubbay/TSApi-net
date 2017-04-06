using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TSApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Enable CORS cross-origin-requests
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // default Json response
            
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Web API configuration and services
            


            // Web API routes
            config.MapHttpAttributeRoutes();

        }
    }
}
