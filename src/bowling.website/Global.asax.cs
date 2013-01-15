using Bowling.Rest.Service.Interface;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TemplateSrc.Init;

namespace Bowling.Web.CustomerSite
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
        public AppHost ServiceAppHost { get; set; }

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);

            DependencyResolverInitializer.Initialize();

            AutomapperConfig.ApplyConfiguration();
		}

        protected void Session_Start(object sender, EventArgs e)
        {

        }
	}
}