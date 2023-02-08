using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GROCERY
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "RouteWithParam",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //   defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultRoute",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "RouteWithParam",
                routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
            );
        }
    }
}
