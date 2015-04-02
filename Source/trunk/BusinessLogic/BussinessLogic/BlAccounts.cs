using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BussinessLogic
{
	public class BlAccounts : BussinessLogicBase
	{
		public BlAccounts(DataStorage storage, int loggedUser)
			: base(storage,loggedUser)
		{
		}

		public AccountInfo Add(NameValueCollection post, out string message)
		{
			string firstName = null, lastName = null, email = null, password = null, avatar = null, role = null;
			int userId = 0;
			bool isExternal = false;
			Status status = Status.Deleted;
			message = "";
			try
			{
				firstName = post["firstname"].Trim();
				lastName = post["lastname"].Trim();
				email = post["email"].Trim();
				password = post["password"].Trim();
				userId = Convert.ToInt32(post["userid"].Trim());
				avatar = post["avatar"].Trim();
				isExternal = Convert.ToBoolean(post["isexternal"].Trim());
				role = post["role"].Trim();
				status = (Status)Convert.ToInt32(post["status"].Trim());
			}
			catch (Exception) { }


			if ((email != null && email != "") && (password != null && password != "") && (firstName != null && firstName != "") 
				&& (lastName != null && lastName != "") && (role != null && role != ""))
			{
				AccountInfo accountInfo = this.DataStorage.AddAccount(email, firstName, lastName, password, userId, avatar,  isExternal, role, status);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public AccountInfo GetWithEmail(string email, out string message)
		{
			message = "";
			if (email != null && email != "")
			{
				AccountInfo accountInfo = this.DataStorage.GetAccountWithEmail(email);

				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public AccountInfo GetWithId(int id, out string message)
		{
			message = "";
			if(id > 0)
			{
				AccountInfo accountInfo = this.DataStorage.GetAccountWithId(id);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}


		public AccountInfo GetOne(out string message)
		{
			message = "";
			if(this.LoggedUser != 0)
			{
				AccountInfo accountInfo = this.DataStorage.GetAccountWithId(this.LoggedUser);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public List<AccountInfo> GetAll(out string message)
		{
			message = "";
			List<AccountInfo> accountInfos = this.DataStorage.GetAllAccountInfo();
			return accountInfos;
		}

		public AccountInfo UpdateName(NameValueCollection post, out string message)
		{
			string firstName = null, lastName = null;
			int accountId = 0;
			message = "";
			try
			{
				accountId = Convert.ToInt32(post["accountid"]);
				firstName = post["firstname"].Trim();
				lastName = post["lastname"].Trim();
			}
			catch (Exception) { }

			
			if ((accountId > 0) && (firstName != null && firstName != "")
				&& (lastName != null && lastName != ""))
			{

				AccountInfo accountInfo = this.DataStorage.UpdateAccountName(accountId, firstName, lastName);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public AccountInfo UpdatePassword(int accountId, string password, out string message)
		{
			message = "";
			if(accountId > 0 && password != null && password != "")
			{
				AccountInfo accountInfo = this.DataStorage.UpdateAccountPassword(accountId, password);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public AccountInfo UpdateUserId(NameValueCollection post, out string message)
		{
			message = "";
			int accountId = 0, userId = 0;
			try
			{
				accountId = Convert.ToInt32(post["accountid"]);
				userId = Convert.ToInt32(post["userid"]);
			}catch(Exception)
			{
				message = __("Parameters is invalid.");
				return null;
			}

			if(accountId > 0 && userId > 0)
			{
				AccountInfo accountInfo = this.DataStorage.UpdateAccountUserId(accountId, userId);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public AccountInfo UpdateAvatar(int accountId, string avatar, out string message)
		{
			message = "";
			if (accountId > 0 && avatar != null && avatar != "")
			{
				AccountInfo accountInfo = this.DataStorage.UpdateAccountAvatar(accountId, avatar);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public AccountInfo UpdateIsExternal(int accountId, bool isExternal, out string message)
		{
			message = "";
			if (accountId > 0)
			{
				AccountInfo accountInfo = this.DataStorage.UpdateAccountIsExternal(accountId, isExternal);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public AccountInfo Upgrade(out string message)
		{
			message = "";
			if(this.LoggedUser != 0)
			{
				AccountInfo accountInfo = this.DataStorage.Upgrade(this.LoggedUser);
				return accountInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}
	}
}
