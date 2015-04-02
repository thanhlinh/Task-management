
namespace Sioux.TaskManagement
{
	public class Permission
	{
		public const string Admin = "Admin";
		public const string User = "User";
		public const string Manager = "Manager";

		public static string[] GetRoles()
		{
			return new string[] {
				Admin,
				Manager,
				User
			};
		}

		protected const string User_And_Manager = User + ", " + Manager;
		protected const string User_And_Manager_And_Admin = User + ", " + Manager + ", " + Admin;

		public class GUI
		{
			public const string Hook = Permission.User_And_Manager;
		}

		public class Accounts
		{
			public const string Add = Permission.Admin;
			public const string GetOne = Permission.User_And_Manager;
			public const string GetAll = Permission.Admin;
			public const string Logout = Permission.User_And_Manager_And_Admin;
			public const string Invite = Permission.User_And_Manager;
			public const string Upgrade = Permission.User;
		}

		public class Boards
		{
			public const string Add = Permission.User_And_Manager;
			public const string Get = Permission.User_And_Manager;
			public const string Update = Permission.User_And_Manager;
			public const string Sync = Permission.User_And_Manager;
		}
		
		public class Columns
		{
			public const string Add = Permission.User_And_Manager;
			public const string Get = Permission.User_And_Manager;
			public const string Update = Permission.User_And_Manager;
			public const string Sync = Permission.User_And_Manager;
		}

		public class Cards
		{
			public const string Add = Permission.User_And_Manager;
			public const string Get = Permission.User_And_Manager;
			public const string Update = Permission.User_And_Manager;
			public const string Sync = Permission.User_And_Manager;
		}

		public class ColumnSetting
		{
			public const string Add = Permission.User_And_Manager;
			public const string Get = Permission.User_And_Manager;
		}
	}
}
