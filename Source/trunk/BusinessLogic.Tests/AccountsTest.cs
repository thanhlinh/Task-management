using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BusinessLogic.Tests
{
	[TestFixture]
	public class AccountsTest : TestBase
	{
		[TestCase]
		public void Add()
		{
			BussinessLogic.BlAccounts accountsLogic = new BussinessLogic.BlAccounts(this.DataStorage, 1);

			string message="";

			NameValueCollection case1 = new NameValueCollection();
			case1["firstname"] = "Phan";
			case1["lastname"] = "HoangNhat";

			AccountInfo account = accountsLogic.Add(case1, out message);
			Assert.IsNull(account);

			NameValueCollection case2 = new NameValueCollection();
			case2["firstname"] = "Quang Huy";
			case2["lastname"] = "Phan";
			case2["email"] = "huy.phan@sioux.asia";
			case2["password"] = "123456";
			case2["userid"] = Convert.ToString(1);

			AccountInfo account2 = accountsLogic.Add(case2, out message);
			Assert.NotNull(account2);
		}
	}
}
