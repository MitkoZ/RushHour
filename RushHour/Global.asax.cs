using RushHour.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RushHour
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CustomAuthorizeAttribute customAuthorizeAttribute = new CustomAuthorizeAttribute();
            GlobalFilters.Filters.Add(customAuthorizeAttribute);
        }
    }
}
