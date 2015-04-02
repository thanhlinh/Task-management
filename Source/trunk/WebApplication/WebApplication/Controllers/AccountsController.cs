using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Sioux.TaskManagement.Controllers
{
	public class AccountsController : AppController
	{

		[Authorize(Roles = Permission.Accounts.GetOne)]
		[HttpGet]
		public ActionResult AccountInfo()
		{
			string message = "";
			BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
			AccountInfo accountInfo = blAccount.GetOne(out message);

			return JsonFormat(accountInfo, message);
		}


		[Authorize(Roles = Permission.Accounts.GetAll)]
		[HttpGet]
		public ActionResult AllAccountInfo()
		{
			string message = "";
			BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
			List<AccountInfo> accountInfos = blAccount.GetAll(out message);

			return JsonFormat(accountInfos, message);
		}

		[Authorize(Roles = Permission.Accounts.Upgrade)]
		[HttpGet]
		public ActionResult Upgrade()
		{
			return View();
		}


		[Authorize(Roles = Permission.Accounts.Upgrade)]
		[HttpPost]
		public ActionResult UpgradeCallback()
		{
			GenericJsonResult result = new GenericJsonResult();

			string message;
			BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
			AccountInfo accountInfo = blAccount.Upgrade(out message);
			if(accountInfo != null)
			{
				Roles.RemoveUserFromRole(WebSecurity.CurrentUserName , Permission.User);
				Roles.AddUsersToRoles(new[] { WebSecurity.CurrentUserName }, new[] { Permission.Manager });
				//Response.Redirect(MagicUrl.GuiHookExt("upgraded"));
				return JavaScript("window.close();");
			}
			else
			{
				return JavaScript("window.close();");
				//Response.Redirect(MagicUrl.GuiHookExt("notupgrade"));
			}
		}

	}
}
