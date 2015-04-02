using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	public class Utility
	{
		protected static DateTime UnixStartTime = new DateTime(1970, 1, 1);

		public static long GetUnixTimestamp(DateTime datetime)
		{
			return (long)(datetime - UnixStartTime).TotalSeconds;
		}

		public static DateTime GetDateTimeFromUnixTimestamp(long unixTimestamp)
		{
			return UnixStartTime.AddSeconds(unixTimestamp);
		}
	}
}
