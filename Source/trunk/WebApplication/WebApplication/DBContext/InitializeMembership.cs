using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sioux.TaskManagement.DBContext;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using WebMatrix.WebData;
using System.Web.Security;

namespace Sioux.TaskManagement.DBContext
{
	public class InitializeMembership
	{
		public InitializeMembership() { }
		public void Init()
		{
			Database.SetInitializer<UsersContext>(null);

			try
			{
				using (var context = new UsersContext())
				{
					if (!context.Database.Exists())
					{
						// Create the SimpleMembership database without Entity Framework migration schema
						((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
					}
				}

				WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "Email", autoCreateTables: true);

				//----------------seed roles------------------//
				foreach (string role in Permission.GetRoles())
				{
					if (!Roles.RoleExists(role))
						Roles.CreateRole(role);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
			}
		}
	}
}