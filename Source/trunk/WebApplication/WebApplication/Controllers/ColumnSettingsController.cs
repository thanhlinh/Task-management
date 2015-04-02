using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sioux.TaskManagement.Controllers
{
    public class ColumnSettingsController : AppController
    {
        [HttpPost]
		[Authorize(Roles = Permission.ColumnSetting.Add)]
		public ActionResult SaveData()
		{
			string message;
			ColumnSettingInfo settingInfo = null;
			try
			{
				settingInfo = JsonConvert.DeserializeObject<ColumnSettingInfo>(Request.Form["data"]);
			}
			catch (Exception) { }
			BussinessLogic.BlColumnSetting blSetting = new BussinessLogic.BlColumnSetting(this.DataStorage, this.AccountId);
			settingInfo = blSetting.SaveData(settingInfo, out message);

			return JsonFormat(settingInfo, message);
		}


		[HttpGet]
		[Authorize(Roles = Permission.ColumnSetting.Get)]
		public ActionResult GetAll()
		{
			string message;
			BussinessLogic.BlColumnSetting blSetting = new BussinessLogic.BlColumnSetting(this.DataStorage, this.AccountId);
			List<ColumnSettingInfo> settingInfos = blSetting.GetAll(Request.QueryString, out message);

			return JsonFormat(settingInfos, message);
		}
    }
}
