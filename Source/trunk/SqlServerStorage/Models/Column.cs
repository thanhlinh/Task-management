using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.Models
{
    [Table("Columns")]
    public class Column
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Owner { get; set; }

        [Required]
        public Guid BoardId { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public int CardCount { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        [Required]
        public Status Status { get; set; }


		//[ForeignKey("BoardId")]
		//public virtual Board Board { get; set; }

		public static implicit operator ColumnInfo(Column column)
		{
			return new ColumnInfo()
			{
				Id = column.Id,
				Name = column.Name,
				Owner = column.Owner,
				BoardId = column.BoardId,
				Priority = column.Priority,
				CardCount = column.CardCount,
				Created = column.Created,
				Updated = column.Updated,
				Status = column.Status
			};
		}
    }
}
