using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	[DataContract()]
	public class ColumnInfo
	{
		public ColumnInfo()
		{

		}

		[DataMember(Name = "id", IsRequired = true)]
		public Guid Id { get; set; }

		[DataMember(Name = "name", IsRequired = true)]
		public string Name { get; set; }

		[DataMember(Name = "owner", IsRequired = true)]
		public int Owner { get; set; }

		[DataMember(Name = "boardId", IsRequired = true)]
		public Guid BoardId { get; set; }

		[DataMember(Name = "cardCount", IsRequired = true)]
		public int CardCount { get; set; }

		[DataMember(Name = "priority", IsRequired = true)]
		public int Priority { get; set; }


		[DataMember(Name = "created", IsRequired = false)]
		public DateTime Created { get; set; }

		[DataMember(Name = "updated", IsRequired = false)]
		public DateTime Updated { get; set; }

		[DataMember(Name = "status", IsRequired = true)]
		public Status Status { get; set; }
	}
}
