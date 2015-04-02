using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.Models
{
    [Table("ColumnSettings")]
    public class ColumnSetting
    {
		[Key]
		public Guid Id { get; set; }

		[Required]
		public Guid ColumnId { get; set; }

		[Required]
		public Guid Next { get; set; }

		[Required]
		public Guid BoardId { get; set; }

		[Required]
		public DateTime Created { get; set; }

		[Required]
		public DateTime Updated { get; set; }

		[Required]
		public Status Status { get; set; }

		//[ForeignKey("ColumnId")]
		//public Column CurrentColumn { get; set; }

		//[ForeignKey("Next")]
		//public Column NextColumn { get; set; }

		public static implicit operator ColumnSettingInfo(ColumnSetting columnSetting)
		{
			return new ColumnSettingInfo()
			{
				Id = columnSetting.Id,
				ColumnId = columnSetting.ColumnId,
				Next = columnSetting.Next,
				BoardId = columnSetting.BoardId,
				Created = columnSetting.Created,
				Updated = columnSetting.Updated,
				Status = columnSetting.Status
			};
		}
    }
}
