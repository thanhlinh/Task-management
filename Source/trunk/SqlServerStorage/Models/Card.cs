using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.Models
{
	[Table("Cards")]
	public class Card
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		public int Owner { get; set; }

		[Required]
		public Guid ColumnId { get; set; }

		[Required]
		public Guid BoardId { get; set; }

		[Required]
		public int Priority { get; set; }

		[Required]
		public string Style { get; set; }

		[Required]
		public int CheckListCount { get; set; }

		[Required]
		public int CommentCount { get; set; }

		[Required]
		public int RunningTimeLog { get; set; }

		[Required]
		public int CurrentEstimated { get; set; }

		[Required]
		public int AttachedCount { get; set; }

		[Required]
		public DateTime Created { get; set; }

		[Required]
		public DateTime Updated { get; set; }

		[Required]
		public Status Status { get; set; }

		//[ForeignKey("ColumnId")]
		//public virtual Column Column { get; set; }

		public static implicit operator CardInfo(Card card)
		{
			return new CardInfo()
			{
				Id = card.Id,
				Name = card.Name,
				Description = card.Description,
				Owner = card.Owner,
				BoardId = card.BoardId,
				ColumnId = card.ColumnId,
				Priority = card.Priority,
				Style = card.Style,
				CheckListCount = card.CheckListCount,
				CommentCount = card.CommentCount,
				RunningTimeLog = card.RunningTimeLog,
				CurrentEstimated = card.RunningTimeLog,
				AttachedCount = card.AttachedCount,
				Created = card.Created,
				Updated = card.Updated,
				Status = card.Status
			};
		}
	}
}
