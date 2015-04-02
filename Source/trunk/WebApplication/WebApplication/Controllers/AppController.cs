using Microsoft.Web.WebPages.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Sioux.TaskManagement.Controllers
{
	public class AppController : Controller
	{
		public AppController()
		{
			DataStorage = MvcApplication.DataStorage;
		}

		public int AccountId
		{
			get
			{
				if (WebSecurity.IsAuthenticated) {
					return (this.DataStorage.GetAccountWithEmail(WebSecurity.CurrentUserName).Id);
				}
				if (OAuthWebSecurity.IsAuthenticatedWithOAuth)
				{
					return (this.DataStorage.GetAccountWithEmail(Session["Email"].ToString()).Id);
				}
				return 0;
			}
		}

		public DataStorage DataStorage { get; protected set; }

		public ActionResult JsonFormat(object data, string message)
		{
			GenericJsonResult result = new GenericJsonResult();

			if (data != null)
			{
				result.Success = true;
				result.Data = data;
			}
			else
			{
				result.Success = false;
				result.Message = message;
			}

			JsonNetResult jsonNetResult = new JsonNetResult();
			jsonNetResult.Formatting = Formatting.Indented;
			jsonNetResult.Data = result;
			return jsonNetResult;
		}

		public ActionResult JsonFormat(object data)
		{
			JsonNetResult jsonNetResult = new JsonNetResult();
			jsonNetResult.Formatting = Formatting.Indented;
			jsonNetResult.Data = data;
			return jsonNetResult;
		}

		public bool IsEmailAvailable(string email)
		{
			bool success = false;

			BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
			string message = "";
			AccountInfo accountInfo = blAccount.GetWithEmail(email, out message);
			success = accountInfo != null;

			return success;
		}

		public bool IsValidEmail(string email)
		{
			string pattern = null;
			pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
			if (Regex.IsMatch(email, pattern))
				return true;
			return false;
		}

		public NameValueCollection CaseAccount(string firstName, string lastName, string email, string password, int userId, string avatar, bool isExternal, string role, Status status)
		{
			NameValueCollection post = new NameValueCollection();

			post["firstname"] = firstName;	
			post["lastname"] = lastName;
			post["email"] = email;
			post["password"] = password;
			post["userid"] = Convert.ToString(userId);
			post["avatar"] = avatar;
			post["isexternal"] = Convert.ToString(isExternal);
			post["role"] = role;
			post["status"] = Convert.ToString((int)status);

			return post;
		}
	}
}