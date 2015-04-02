using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sioux.TaskManagement
{
	public class MagicUrl
	{
		public static string GuiHookExt(string cmd, string data = "", bool close = true)
		{
			StringBuilder url = new StringBuilder();
			url.Append("/ws/hook-ext");
			url.Append("?cmd=");
			url.Append(HttpUtility.UrlEncode(cmd));
			if(!string.IsNullOrEmpty(data)) {
				url.Append("&data=");
				url.Append(HttpUtility.UrlEncode(data));
			}
			if (close)
				url.Append("&close=true");
			else
				url.Append("&close=false");
			return url.ToString();
		}

		public static string ApiAccountInfo()
		{
			return "/ws/account-info";
		}

		public static string ThankYouPage()
		{
			return "/home/thankyou";
		}

		public static string ExternalLogin(string provider, string returnUrl)
		{
			StringBuilder url = new StringBuilder();

			url.Append("externallogin");
			url.Append("?provider=");
			url.Append(HttpUtility.UrlEncode(provider));

			if (!string.IsNullOrEmpty(returnUrl.Trim()))
			{
				url.Append("&returnUrl=");
				url.Append(HttpUtility.UrlEncode(returnUrl.Trim()));
			}

			return url.ToString();
		}

		public static string LoginPage()
		{
			return "/login/login";
		}
	}
}