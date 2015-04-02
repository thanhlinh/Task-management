using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	[DataContract()]
	public class GenericJsonResult
	{
		public GenericJsonResult()
		{
			this.Success = false;
		}

		public GenericJsonResult(bool success)
		{
			this.Success = success;
		}

		public GenericJsonResult(bool success, object data)
		{
			this.Success = success;
			this.Data = data;
		}

		public GenericJsonResult(bool success, object data, string message)
		{
			this.Success = success;
			this.Data = data;
			this.Message = message;
		}

		[DataMember(Name = "success", IsRequired = true)]
		public bool Success { get; set; }

		[DataMember(Name = "data", IsRequired = false, EmitDefaultValue = true)]
		public object Data { get; set; }


		[DataMember(Name = "message", IsRequired = false, EmitDefaultValue = true)]
		public string Message { get; set; }

	}
}
