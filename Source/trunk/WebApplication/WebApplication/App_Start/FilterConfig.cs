using System.Web;
using System.Web.Mvc;

namespace Sioux.TaskManagement
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}