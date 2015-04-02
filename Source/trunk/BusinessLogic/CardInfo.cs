using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	[DataContract()]
	public class CardInfo
	{
		public CardInfo() { }

		[DataMember(Name = "id", IsRequired = true)]
		public Guid Id { get; set; }

		[DataMember(Name = "name", IsRequired = true)]
		public string Name { get; set; }

		[DataMember(Name = "description", IsRequired = true)]
		public string Description { get; set; }

		[DataMember(Name = "owner", IsRequired = true)]
		public int Owner { get; set; }

		[DataMember(Name = "boardId", IsRequired = true)]
		public Guid BoardId { get; set; }

		[DataMember(Name = "columnId", IsRequired = true)]
		public Guid ColumnId { get; set; }

		[DataMember(Name = "priority", IsRequired = true)]
		public int Priority { get; set; }

		[DataMember(Name = "style", IsRequired = false)]
		public string Style { get; set; }

		[DataMember(Name = "checklistCount", IsRequired = true)]
		public int CheckListCount { get; set; }

		[DataMember(Name = "commentCount", IsRequired = true)]
		public int CommentCount { get; set; }

		[DataMember(Name = "runningTimeLog", IsRequired = true)]
		public int RunningTimeLog { get; set; }

		[DataMember(Name = "currentEstimated", IsRequired = true)]
		public int CurrentEstimated { get; set; }

		[DataMember(Name = "attachedCount", IsRequired = true)]
		public int AttachedCount { get; set; }

		[DataMember(Name = "created", IsRequired = false)]
		public DateTime Created { get; set; }

		[DataMember(Name = "updated", IsRequired = false)]
		public DateTime Updated { get; set; }

		[DataMember(Name = "status", IsRequired = true)]
		public Status Status { get; set; }
	}
}
