using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	[DataContract()]
	public class BoardInfo
	{
		public BoardInfo()
		{

		}

		[DataMember(Name = "id", IsRequired = true)]
		public Guid Id { get; set; }

		[DataMember(Name = "name", IsRequired = true)]
		public string Name { get; set; }

		[DataMember(Name = "owner", IsRequired = true)]
		public int Owner { get; set; }

		[DataMember(Name = "priority", IsRequired = false)]
		public int Priority { get; set; }

		[DataMember(Name = "columnCount", IsRequired = true)]
		public int ColumnCount { get; set; }

		[DataMember(Name = "created", IsRequired = false)]
		public DateTime Created { get; set; }

		[DataMember(Name = "updated", IsRequired = false)]
		public DateTime Updated { get; set; }

		[DataMember(Name = "status", IsRequired = true)]
		public Status Status { get; set; }

		[DataMember(Name = "hashcode", IsRequired = true)]
		public string HashCode { get; set; }
	}
}
