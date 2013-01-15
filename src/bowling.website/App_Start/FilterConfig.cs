using System.Web;
using System.Web.Mvc;

namespace Bowling.Web.CustomerSite
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}