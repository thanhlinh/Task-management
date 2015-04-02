using Sioux.TaskManagement.DBContext;
using Sioux.TaskManagement.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Web;

namespace TaskManagement.Helper
{
    class EmailHelper
    {
        public static void SendConfirmationEmail(string emailto, string username, string hosturl)
        {
            string smtpAddress = "mail01.sioux.eu";
            int portNumber = 5506;
            bool enableSSL = false;

            string emailFrom = "noreply@sioux.asia";
            string password = "xE5j9v1NVXQ4*";
            string emailTo = emailto;
            string subject = "Please confirm your email.";
			string path = "~/Views/EmailTemplate/EmailConfirm.html";
            string body = PopulateBody(username, "", "", hosturl, path);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }

		public static void SendInviteEmail(string emailto, string usernameTo, string usernameFrom, string message, string hosturl)
		{
			string smtpAddress = "mail01.sioux.eu";
			int portNumber = 5506;
			bool enableSSL = false;

			string emailFrom = "noreply@sioux.asia";
			string password = "xE5j9v1NVXQ4*";
			string emailTo = emailto;
			string subject = "Please confirm your email.";
			string path = "~/Views/EmailTemplate/EmailInvite.html";
			string body = PopulateBody(usernameTo, usernameFrom, message, hosturl, path );

			using (MailMessage mail = new MailMessage())
			{
				mail.From = new MailAddress(emailFrom);
				mail.To.Add(emailTo);
				mail.Subject = subject;
				mail.Body = body;
				mail.IsBodyHtml = true;
				// Can set to false, if you are sending pure text.

				using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
				{
					smtp.Credentials = new NetworkCredential(emailFrom, password);
					smtp.EnableSsl = enableSSL;
					smtp.Send(mail);
				}
			}
		}


        public static string GetEmail(int userId)
        {
            string email = "";
            using (UsersContext db = new UsersContext())
            {
                UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserId == userId);
				if(user != null)
				{
					email = user.Email.ToString();
				}  
            }; 
            return email;
        }

		private static string PopulateBody(string usernameTo, string usernameFrom, string message, string url, string path)
		{
			string body = string.Empty;
			using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
			{
				body = reader.ReadToEnd();
			}
			body = body.Replace("{UserNameTo}", usernameTo);
			body = body.Replace("{UserNameFrom}", usernameFrom);
			body = body.Replace("{Message}", message);
			body = body.Replace("{Url}", url);
			return body;
		}
    }
}
