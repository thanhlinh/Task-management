using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sioux.TaskManagement
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			rwUrl("ws/log-out", "Login", "LogOff", routes);

			rwUrl("ws/invite", "Invite", "Invite", routes);

			rwUrl("ws/account-info", "Accounts", "AccountInfo", routes);
			rwUrl("ws/all-account-info", "Accounts", "AllAccountInfo", routes);
			rwUrl("ws/upgrade", "Accounts", "Upgrade", routes);

			rwUrl("ws/add-board", "Boards", "Add", routes);
			rwUrl("ws/get-board", "Boards", "GetOne", routes);
			rwUrl("ws/get-all-board", "Boards", "GetAll", routes);
			rwUrl("ws/save-board", "Boards", "SaveData", routes);
			rwUrl("ws/sync/board", "Boards", "Sync", routes);

			rwUrl("ws/save-column", "Columns", "SaveData", routes);
			rwUrl("ws/get-column", "Columns", "GetOne", routes);
			rwUrl("ws/get-all-column", "Columns", "GetAll", routes);

			rwUrl("ws/save-card", "Cards", "SaveData", routes);
			rwUrl("ws/get-card", "Cards", "GetOne", routes);
			rwUrl("ws/get-all-card", "Cards", "GetAll", routes);


			rwUrl("ws/save-columnsetting", "ColumnSettings", "SaveData", routes);
			rwUrl("ws/get-all-columnsetting", "ColumnSettings", "GetAll", routes);

			rwUrl("ws/hook-ext", "GUI", "Hook", routes);

			rwUrl("hashcode/board", "Hashcode", "Board", routes);
			rwUrl("hashcode/column", "Hashcode", "Column", routes);
			rwUrl("hashcode/card", "Hashcode", "Card", routes);
			rwUrl("hashcode/column-setting", "Hashcode", "ColumnSetting", routes);

			routes.MapRoute(
					name: "Default",
					url: "{controller}/{action}/{id}",
					defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}

		protected static void rwUrl(string url, string controller, string action, RouteCollection routes, string name = null)
		{
			string routeName = !string.IsNullOrEmpty(name) ? controller + action : name;
			routes.MapRoute(
					name: routeName,
					url: url,
					defaults: new { controller = controller, action = action }
			);
		}
	}
}