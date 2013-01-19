using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvcSample.Controllers;
using NavigationRoutes;
using bowling.administration.website.Controllers;

namespace BootstrapMvcSample
{
    public class MenuConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
			//routes.MapNavigationRoute<HomeController>("Automatic Scaffolding", c => c.Index());

			routes.MapNavigationRoute<HomeController>("Manage stuff", c => c.Index())
				.AddChildRoute<TimeSlotController>("Time slots", c => c.Index())
				.AddChildRoute<LaneController>("Lanes", c => c.Index());
        }
    }
}
