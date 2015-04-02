using Sioux.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	public partial class SqlServerStorage : DataStorage
	{
		TaskManagementDBEntities dbContext = new TaskManagementDBEntities();

		public override AccountInfo GetAccountWithId(int accountId)
		{
			Account account = dbContext.Accounts.Where(a => a.Id == accountId && (a.Status == Status.Enable || a.Status == Status.Disabled))
				.FirstOrDefault();
			try
			{
				return (AccountInfo)account;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public override AccountInfo GetAccountWithUserId(int userId)
		{
			Account account = dbContext.Accounts.Where(a => a.UserId == userId && a.Status == Status.Enable)
				.FirstOrDefault();
			try
			{
				return (AccountInfo)account;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public override AccountInfo GetAccountWithEmail(string email)
		{
			Account account = dbContext.Accounts.Where(a => a.Email.Replace(".","").Equals(email.Replace(".","")) && a.Status != Status.Deleted).FirstOrDefault();

			try
			{
				return (AccountInfo)account;
			}catch(Exception)
			{
				return null;
			}
			
		}

		public override List<AccountInfo> GetAllAccountInfo()
		{
			List<Account> accounts = dbContext.Accounts.Where(a => a.Status == Status.Enable).ToList();
			List<AccountInfo> accountInfos = new List<AccountInfo>();
			foreach(Account account in accounts)
			{
				accountInfos.Add((AccountInfo)account);
			}

			return accountInfos;
		}
	
		public override AccountInfo AddAccount(string email, string firstName, string lastName, string password, int userId, string avatar, bool isExternal, string role, Status status)
		{
			Account account = new Account()
			{
				UserId = userId,
				FirstName = firstName,
				LastName = lastName,
				Email = email,
				Password = password,
				AccountType = AccountType.Basic,
				Secret = Guid.Empty,
				Code = Guid.NewGuid(),
				Avatar = avatar,
				IsExternal = isExternal,
				Role = role,
				Created = DateTime.UtcNow,
				Updated = DateTime.UtcNow,
				Status = status
			};
			try
			{
				dbContext.Accounts.Add(account);
				dbContext.SaveChanges();
				return (AccountInfo)account;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public override AccountInfo UpdateAccountUserId(int accountId, int userId)
		{
			Account account = dbContext.Accounts.Where(a => a.Id == accountId).FirstOrDefault();

			if (account != null)
			{
				account.UserId = userId;
				account.Updated = DateTime.UtcNow;

				try
				{
					dbContext.SaveChanges();
					return (AccountInfo)account;
				}
				catch (Exception) { }
			}
			return null;
		}

		public override AccountInfo Upgrade(int accountId)
		{
			Account account = dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);
			if(account != null)
			{
				account.Role = Permission.Manager;
				account.Updated = DateTime.UtcNow;
				try
				{
					dbContext.SaveChanges();
					AccountInfo accountInfo = (AccountInfo)account;
					return accountInfo;
				}
				catch (Exception) { }
			}

			return null;
		}

		public override AccountInfo UpdateAccountName(int accountId, string firstName, string lastName)
		{
			Account account = dbContext.Accounts.Where(a => a.Id == accountId).FirstOrDefault();

			if(account != null)
			{
				account.FirstName = firstName;
				account.LastName = lastName;
				account.Updated = DateTime.UtcNow;

				try
				{
					dbContext.SaveChanges();
					return (AccountInfo)account;
				}
				catch (Exception) { }
			}

			return null;
		}

		public override AccountInfo UpdateAccountPassword(int accountId, string password)
		{
			Account account = dbContext.Accounts.Where(a => a.Id == accountId).FirstOrDefault();

			if (account != null)
			{
				account.Password = password;
				account.Updated = DateTime.UtcNow;

				try
				{
					dbContext.SaveChanges();
					return (AccountInfo)account;
				}
				catch (Exception) { }
			}

			return null;
		}

		public override AccountInfo UpdateAccountIsExternal(int accountId, bool isExternal)
		{
			Account account = dbContext.Accounts.Where(a => a.Id == accountId).FirstOrDefault();

			if (account != null)
			{
				account.IsExternal = isExternal;
				account.Updated = DateTime.UtcNow;

				try
				{
					dbContext.SaveChanges();
					return (AccountInfo)account;
				}
				catch (Exception) { }
			}

			return null;
		}

		public override AccountInfo UpdateAccountAvatar(int accountId, string avatar)
		{
			Account account = dbContext.Accounts.Where(a => a.Id == accountId).FirstOrDefault();

			if (account != null)
			{
				account.Avatar = avatar;
				account.Updated = DateTime.UtcNow;

				try
				{
					dbContext.SaveChanges();
					return (AccountInfo)account;
				}
				catch (Exception) { }
			}

			return null;
		}

		public override AccountInfo UpdateAccountStatus(int accountId, Status status)
		{
			Account account = dbContext.Accounts.Where(a => a.Id == accountId).FirstOrDefault();

			if (account != null)
			{
				account.Status = status;
				account.Updated = DateTime.UtcNow;

				try
				{
					dbContext.SaveChanges();
					return (AccountInfo)account;
				}
				catch (Exception) { }
			}

			return null;
		}
	}
}
