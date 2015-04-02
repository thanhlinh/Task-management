using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	[DataContract()]
	public class ColumnSettingInfo
	{
		public ColumnSettingInfo() { }

		[DataMember(Name = "id", IsRequired = false)]
		public Guid Id { get; set; }

		[DataMember(Name = "columnId", IsRequired = true)]
		public Guid ColumnId { get; set; }

		[DataMember(Name = "next", IsRequired = true)]
		public Guid Next { get; set; }

		[DataMember(Name = "boardId", IsRequired = true)]
		public Guid BoardId { get; set; }

		[DataMember(Name = "created", IsRequired = false)]
		public DateTime Created { get; set; }

		[DataMember(Name = "updated", IsRequired = false)]
		public DateTime Updated { get; set; }

		[DataMember(Name = "status", IsRequired = true)]
		public Status Status { get; set; }
	}
}
