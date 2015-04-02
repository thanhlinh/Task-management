using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BussinessLogic
{
	public class BussinessLogicBase
	{
		public BussinessLogicBase(DataStorage storage, int loggedUser)
		{
			this.DataStorage = storage;
			this.LoggedUser = loggedUser;
		}

		public DataStorage DataStorage { get; protected set; }

		public int LoggedUser { get; protected set; }

		protected string __(string text)
		{
			return text;
		}
	}
}
