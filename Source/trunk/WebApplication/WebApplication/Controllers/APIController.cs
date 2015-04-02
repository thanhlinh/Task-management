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
	public class APIController : AppController
	{

		[AllowAnonymous]
		public ActionResult AccountInfo()
		{
			GenericJsonResult result = new GenericJsonResult();
			AccountInfo accountInfo = DataStorage.GetAccountInfo(this.AccountId);
			if (accountInfo == null)
			{
				result.Success = false;
			}
			else
			{
				result.Success = true;
				result.Data = accountInfo;
			}


			//JsonNetResult jsonNetResult = new JsonNetResult();
			//jsonNetResult.Formatting = Formatting.Indented;
			//jsonNetResult.Data = accountInfo;

			//return jsonNetResult;

			return new JsonNetResult(result);
		}


		[AllowAnonymous]
		public ActionResult AllAccountInfo()
		{
			GenericJsonResult result = new GenericJsonResult();
			List<AccountInfo> accountInfos = DataStorage.GetAllAccountInfo();

			JsonNetResult jsonNetResult = new JsonNetResult();
			jsonNetResult.Formatting = Formatting.Indented;
			jsonNetResult.Data = accountInfos;

			return jsonNetResult;
		}

		[AllowAnonymous]
		public ActionResult Add()
		{
			GenericJsonResult result = new GenericJsonResult();

			string message;
			BussinessLogic.Boards blBoard = new BussinessLogic.Boards(this.DataStorage, this.AccountId);

			BoardInfo board = blBoard.Add(Request.Form, out message);
			if (board != null)
			{
				result.Success = true;
				result.Data = board;
			}
			else
			{
				result.Success = false;
				result.Message = message;
			}
			return JsonFormat(result, message);
		}

	}
}
