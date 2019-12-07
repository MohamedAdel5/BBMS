using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BBMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //"ShiftIndex",                                              // Route name
            //"{controlle}/{action}/{blood_camp_id}/{shift_date}",                           // URL with parameters
            //new { controller = "Shift", action = "Details", blood_camp_id = "", shift_date = "" }  // Parameter defaults
        //);

        }
    }
}
