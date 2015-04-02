using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sioux.TaskManagement.Controllers
{
    public class ColumnsController : AppController
    {
		[HttpPost]
		[Authorize(Roles = Permission.Columns.Add)]
		public ActionResult SaveData()
		{
			string message;
			ColumnInfo columnInfo = null;
			try
			{
				string json = Request.Form["data"];
				columnInfo = JsonConvert.DeserializeObject<ColumnInfo>(json);
			}
			catch (Exception) { }

			BussinessLogic.BlColumns blColumn = new BussinessLogic.BlColumns(this.DataStorage, this.AccountId);
			columnInfo = blColumn.SaveData(columnInfo, out message);

			return JsonFormat(columnInfo, message);
		}

		[HttpGet]
		[Authorize(Roles = Permission.Columns.Get)]
		public ActionResult GetOne()
		{
			string message;
			BussinessLogic.BlColumns blColumn = new BussinessLogic.BlColumns(this.DataStorage, this.AccountId);
			ColumnInfo columnInfo = blColumn.GetOne(Request.QueryString, out message);

			return JsonFormat(columnInfo, message);
		}


		[HttpGet]
		[Authorize(Roles = Permission.Columns.Get)]
		public ActionResult GetAll()
		{
			string message;
			BussinessLogic.BlColumns blColumn = new BussinessLogic.BlColumns(this.DataStorage, this.AccountId);
			List<ColumnInfo> columnInfos = blColumn.GetAll(Request.QueryString, out message);

			return JsonFormat(columnInfos, message);
		}

    }
}
