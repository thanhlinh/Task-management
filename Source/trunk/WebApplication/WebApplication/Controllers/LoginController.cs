using Sioux.TaskManagement.DBContext;
using Sioux.TaskManagement.Models;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagement.Helper;
using WebMatrix.WebData;
using System.Collections.Specialized;
using Facebook;
namespace Sioux.TaskManagement.Controllers
{
	[Authorize]
	//[InitializeSimpleMembership]
	public class LoginController : AppController
	{

		#region Login
		[AllowAnonymous]
		public ActionResult Login(string popup)
		{
			try
			{
				Session["Popup"] = popup;
			}catch(Exception){
				Session["Popup"] = "no";
			}

			return View();
		}


		[HttpPost]
		[AllowAnonymous]
		public ActionResult Login(string email, string password)
		{
			GenericJsonResult result = new GenericJsonResult();
			email = email.Trim().ToLower();
		
			if (ModelState.IsValid && WebSecurity.Login(email, password, true))
			{
				result.Success = true;
				result.Data = Constant.Success_Successful;
				string popup = null;
				try
				{
					Session["Popup"].ToString();
				}
				catch (Exception) { }
				if (popup != null && popup.ToLower().Equals("true"))
				{
					result.Message = "popup";
				}
				else result.Message = "nopopup";
			} else if (WebSecurity.UserExists(email) && !WebSecurity.IsConfirmed(email))
			{
				result.Success = false;
				result.Data = Constant.Error_UserUnconfirmed;
			}
			return JsonFormat(result);
		}


		[Authorize(Roles = Permission.Accounts.Logout)]
		public void LogOff()
		{
			WebSecurity.Logout();
			Session.RemoveAll();
			Response.Redirect(MagicUrl.GuiHookExt("logout"));
		}
		#endregion
		

		#region Register
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Register(string email, string firstName, string lastName, string password)
		{
			GenericJsonResult result = new GenericJsonResult();
			email = email.Trim().ToLower();
			string avatar = "https://cdn2.iconfinder.com/data/icons/danger-problems/512/anonymous-512.png";
			bool isExternal = false;

			if (ModelState.IsValid)
			{
				if(IsValidEmail(email))
				{
					if (!IsEmailAvailable(email))
					{
						try
						{
							string confirmationtoken = WebSecurity.CreateUserAndAccount(email, password, null, true);
							int userId = WebSecurity.GetUserId(email);

							Roles.AddUsersToRoles(new[] { email }, new[] { Permission.User });

							BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
							NameValueCollection post = CaseAccount(firstName.Trim(), lastName.Trim(), email, password, userId, avatar, isExternal, Permission.User, Status.Enable);

							string message;
							string userName = firstName + ' ' + lastName;

							AccountInfo accountInfo = blAccount.Add(post, out message);


							if (confirmationtoken != null && accountInfo != null)
							{
								string hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
								   "/Login/RegistrationConfirmation?id=" + userId.ToString() +
								   "&token=" + confirmationtoken;
								EmailHelper.SendConfirmationEmail(email, userName, hosturl);
								string msg = string.Format("Hello {0}, thank you for registration." +
								   "To your email address the message was sent. " +
								   "Click link to finalize of registration process.",
								   email);
								Session["Message"] = msg;
								result.Success = true;
								result.Message = Constant.Success_Successful;
							}
							else
							{
								result.Success = false;
								result.Message = Constant.Error_Ajax;
							}
							
						}
						catch (MembershipCreateUserException)
						{

						}
					}
					else
					{
						result.Success = true;
						result.Message = Constant.Error_EmailAvailable;
					}
				}
				else
				{
					result.Success = true;
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
		public ActionResult RegistrationConfirmation(int id, string token)
		{
			var confirmationtoken = Request["token"];
			string msg = "Your account was not confirmed. " +
			   "Please try again or contact the web site administrator.";
			if (!String.IsNullOrEmpty(confirmationtoken))
			{
				var email = ConfirmAccount(id, confirmationtoken);
				string message = "";
				BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
				AccountInfo accountInfo = blAccount.GetWithEmail(email, out message);

				if(accountInfo != null)
				{
					NameValueCollection caseUserId = new NameValueCollection();
					caseUserId["accountid"] = Convert.ToString(accountInfo.Id);
					caseUserId["userid"] = Convert.ToString(id);

					accountInfo = blAccount.UpdateUserId(caseUserId, out message);

					if (!String.IsNullOrEmpty(email) && accountInfo != null)
					{
						msg = string.Format("Welcome {0}! Registration completed successfully. " +
							"Possible to access the service by logging.",
							email);
					}
				}				
			}

			Session["Message"] = msg;
			return Redirect(MagicUrl.ThankYouPage());
		}

		private string ConfirmAccount(int userid, string token)
		{
			string email = "";
			using (UsersContext db = new UsersContext())
			{
				try
				{
					bool b = WebSecurity.ConfirmAccount(token);
					if (b)
					{
						email = EmailHelper.GetEmail(userid);
					}
				}
				catch (Exception)
				{
				}
			}
			return email;
		}
		#endregion  Register	

		[AllowAnonymous]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
		}

		[AllowAnonymous]
		public ActionResult ExternalLoginCallback(string returnUrl)
		{
			AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
			if (!result.IsSuccessful)
			{
				return RedirectToAction("ExternalLoginFailure");
			}

			Session["Email"] = result.UserName.ToLower();

			if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
			{
				string popup = null;
				try
				{
					popup = Session["Popup"].ToString();
				}
				catch (Exception) { }
				if (popup != null && popup.ToLower().Equals("true"))
				{
					return Redirect(MagicUrl.GuiHookExt("logged"));	
				}else return Redirect("/webapp/");	
			}

			if (User.Identity.IsAuthenticated)
			{
				// If the current user is logged in add the new account
				OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
				return RedirectToLocal(returnUrl);
			}
			else
			{
				string email = result.UserName.ToLower();
				string provider = result.Provider;
				string providerUId = result.ProviderUserId;

				string firstName = "Guest", lastName = "Guest";
				string password = Membership.GeneratePassword(6, 2);
				string url = "https://graph.facebook.com/";
				string avatar = "https://cdn2.iconfinder.com/data/icons/danger-problems/512/anonymous-512.png";
				bool isFb = false, isGg = false;
				string message = "";

				//------------------------
				
				if (provider.Equals("facebook"))
				{
					var client = new FacebookClient(result.ExtraData["accesstoken"].ToString());
					dynamic fbresult = client.Get("me");
					FacebookUserModel facebookUser = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookUserModel>(fbresult.ToString());

					firstName = facebookUser.first_name;
					lastName = facebookUser.last_name;
					avatar = url + "/" + facebookUser.id + "/picture";
					isFb = true;
				}
				 if(provider.Equals("google"))
				 {
					 firstName = result.ExtraData["firstName"];
					 lastName = result.ExtraData["lastName"];
					 isGg = true;
				 }

				using (UsersContext db = new UsersContext())
				{
					if (!IsEmailAvailable(result.UserName.Trim()))
					{
						WebSecurity.CreateUserAndAccount(email, password);

						Roles.AddUsersToRoles(new[] { email }, new[] { Permission.User });
						UserProfile user = db.UserProfiles.FirstOrDefault(u => u.Email.ToLower() == email);

						BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
						NameValueCollection post = CaseAccount(firstName, lastName, email, password, user.UserId, avatar, true, Permission.User, Status.Enable);
						
						blAccount.Add(post, out message);
					}
					else
					{
						if(isGg || isFb)
						{
							BussinessLogic.BlAccounts blAccount = new BussinessLogic.BlAccounts(this.DataStorage, this.AccountId);
							AccountInfo accountInfo = blAccount.GetWithEmail(email, out message);

							if (isFb)
							{
								accountInfo = blAccount.UpdateAvatar(accountInfo.Id, avatar, out message);
							}

							if (accountInfo.FirtsName.Equals("Guest") || accountInfo.LastName.Equals("Guest"))
							{
								NameValueCollection post = new NameValueCollection();
								post["accountid"] = accountInfo.Id.ToString();
								post["firstname"] = firstName;
								post["lastname"] = lastName;

								accountInfo = blAccount.UpdateName(post, out message);
							}
						}
					}
					OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUId, email);
					OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, false);

					string popup = null;
					try
					{
						popup = Session["Popup"].ToString();
					}
					catch (Exception) { }
					if (popup != null && popup.ToLower().Equals("true"))
					{
						return Redirect(MagicUrl.GuiHookExt("logged"));
					}
					else return Redirect("/webapp/");					
				}
			}
		}


		internal class ExternalLoginResult : ActionResult
		{
			public ExternalLoginResult(string provider, string returnUrl)
			{
				Provider = provider;
				ReturnUrl = returnUrl;
			}

			public string Provider { get; private set; }
			public string ReturnUrl { get; private set; }

			public override void ExecuteResult(ControllerContext context)
			{
				OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}
	}
}
