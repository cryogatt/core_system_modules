using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using CryogattServerAPI.Formatters;

namespace CryogattServerAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
                        
            // Always pass bad request messages to the client
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Use customer formatter for csv
            config.Formatters.Add(new ExternalDataCSVFormatter());

            // Use JSON
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
                                                          .Add(new RequestHeaderMapping("Accept",
                                                          "text/html",
                                                          StringComparison.InvariantCultureIgnoreCase,
                                                          true,
                                                          "application/json"));
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ExportApi",
                routeTemplate: "api/v1/exportRecords",
                defaults: new { controller = "ImportExport" }
            );
            //config.Routes.MapHttpRoute(
            //    name: "ExportApi",
            //    routeTemplate: "api/v1/exportRecordsCropFilter",
            //    defaults: new { controller = "ImportExport" }
            //);
            config.Routes.MapHttpRoute(
                name: "ImportApi",
                routeTemplate: "api/v1/importRecords",
                defaults: new { controller = "ImportExport" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{uid}",
                defaults: new { uid = RouteParameter.Optional }
            );
            
            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/v1/users/tokens",
                defaults: new { controller = "Tokens" }
            );
        }
    }
}
