using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagement.Helper;
using WebMatrix.WebData;

namespace Sioux.TaskManagement.Controllers
{
    public class InviteController : AppController
    {
		[HttpGet]
		[Authorize(Roles = Permission.Accounts.Invite)]
		public ActionResult Invite()
		{
			return View();
		}


		[HttpPost]
		[Authorize(Roles = Permission.Accounts.Invite)]
		public ActionResult Invite(string email, string firstName, string lastName, string message)
		{
			GenericJsonResult result = new GenericJsonResult();
			email = email.Trim().ToLower();
			firstName = firstName.Trim();
			lastName = lastName.Trim();
			message = message.Trim();
			string msg;

			if (ModelState.IsValid)
			{
				if (IsValidEmail(email))
				{
					if (!IsEmailAvailable(email))
					{
						BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
						if (firstName == "") firstName = "Guest";
						if (lastName == "") lastName = "Guest";
						string password = Membership.GeneratePassword(6, 2);
						string avatar = "https://fbcdn-sphotos-a-a.akamaihd.net/hphotos-ak-xfp1/v/t1.0-9/283270_257757160902107_690367_n.jpg?oh=96ef018f83d4691ec7540725cca7a1fc&oe=55627999&__gda__=1431363730_c27192f2fbabb0506b1de21f2bc2b7a9";
						int userId = 0;
						NameValueCollection post = CaseAccount(firstName, lastName, email, password, userId, avatar, false, Permission.User, Status.Disabled);
						AccountInfo accountInfo = blAccount.Add(post, out msg);
						AccountInfo userInfo = blAccount.GetWithEmail(WebSecurity.CurrentUserName, out msg);
						if (accountInfo != null)
						{
							result.Success = true;
							result.Message = Constant.Success_Successful;

							msg = "You've sent an invitation email to " + email + ", thank you.";
							Session["Message"] = msg;

							Guid code = accountInfo.Code;
							int id = accountInfo.Id;
							string usernameTo = accountInfo.FirtsName;
							string usernameFrom = userInfo.Email;

							if (usernameTo.Equals("Guest"))
							{
								usernameTo = email;
							}
							string hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
								"/Invite/InviteConfirmation?id=" + id +
								"&code=" + code.ToString();
							EmailHelper.SendInviteEmail(email, usernameTo, usernameFrom, message, hosturl);
						}
						else
						{
							result.Success = true;
							result.Message = Constant.Error_Ajax;
						}
					}
					else
					{
						result.Success = false;
						result.Message = Constant.Error_EmailAvailable;
					}
				}
				else
				{
					result.Success = false;
					result.Message = Constant.Error_EmailInValid;
				}
			}
			else
			{
				result.Success = false;
				result.Message = Constant.Error_Ajax;
			}

			return JsonFormat(result);
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult InviteConfirmation(int id, string code)
		{
			BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
			string message = "";
			AccountInfo accountInfo = blAccount.GetWithId(id, out message);
			try
			{
				if (WebSecurity.UserExists(accountInfo.Email))
				{
					message = "You've completed registration.";
					Session["Message"] = message;

					return Redirect(MagicUrl.ThankYouPage());
				}
			}
			catch (Exception) { }
			

			ViewBag.Email = accountInfo.Email;
			ViewBag.FirstName = accountInfo.FirtsName;
			ViewBag.LastName = accountInfo.LastName;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult InviteConfirmation(string email, string password, string firstName, string lastName)
		{
			GenericJsonResult result = new GenericJsonResult();
			string msg = "";
			if (ModelState.IsValid)
			{
				try
				{
					WebSecurity.CreateUserAndAccount(email, password);
					int userId = WebSecurity.GetUserId(email);

					Roles.AddUsersToRoles(new[] { email }, new[] { Permission.User });

					BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
					AccountInfo accountInfo = blAccount.GetWithEmail(email, out msg);

					NameValueCollection caseInvite = new NameValueCollection();
					caseInvite["accountid"] = accountInfo.Id.ToString();
					caseInvite["firstname"] = firstName;
					caseInvite["lastname"] = lastName;
					string message = "";

					accountInfo = blAccount.UpdateName(caseInvite, out message);

					NameValueCollection caseUserId = new NameValueCollection();
					caseUserId["accountid"] = accountInfo.Id.ToString();
					caseUserId["userid"] = userId.ToString();

					accountInfo = blAccount.UpdateUserId(caseUserId, out message);
					if (accountInfo != null)
					{

						msg = "Thank you for your cooporation. Now, you can use our softwave with your email address.";
						Session["Message"] = msg;
						result.Success = true;
						result.Message = Constant.Success_Successful;
					}
				}
				catch (Exception) { }
			}
			else
			{
				result.Success = false;
				result.Message = Constant.Error_Ajax;
			}

			return JsonFormat(result);
		}

    }
}
