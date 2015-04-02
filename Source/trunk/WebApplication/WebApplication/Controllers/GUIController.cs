using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Sioux.TaskManagement.Controllers
{
	public class GUIController : AppController
	{
		//[Authorize(Roles = Permission.GUI.Hook)]
		public ActionResult Hook(string cmd = "", string data = "", string close = "")
		{
			string command = cmd;
			string base64Data = "";
			int isClose = 0;
			if (data != "")
			{
				base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
			}
			switch (close.ToLower())
			{
				case "true":
				case "close":
				case "ok":
				case "1":
					isClose = 1;
					break;
				default:
					isClose = 0;
					break;
			}
						
			ViewBag.cmd = command;
			ViewBag.data = base64Data;
			ViewBag.close = isClose;
			return View();
		}
	}
}
