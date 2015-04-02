#if DEBUG
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Sioux.TaskManagement.Controllers
{
	public class DebugController : AppController
	{
		[Authorize(Roles = Permission.GUI.Hook)]
		public ActionResult AddBoard(string type = "", string data = "")
		{
			return View();
		}
	}
}
#endif