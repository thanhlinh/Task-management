using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.Models
{
	[Table("Boards")]
	public class Board
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int Owner { get; set; }

		[Required]
		public int Priority { get; set; }

		[Required]
		public int ColumnCount { get; set; }

		[Required]
		public DateTime Created { get; set; }

		[Required]
		public DateTime Updated { get; set; }

		[Required]
		public Status Status { get; set; }

		public string HashCode { get; set; }

		public string CalcHashcode()
		{
			MD5 md5 = MD5.Create();
			Guid hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(
				FingerString
			)));
			this.HashCode = hash.ToString("N");
			return this.HashCode;
		}

		public string FingerString
		{
			get
			{
				return
					this.Id.ToString() + "|" +
					this.Name + "|" +
					this.Priority + "|" +
					this.ColumnCount + "|" +
					this.Created + "|" +
					this.Updated + "|" +
					this.Status;
			}
		}

		//[ForeignKey("OwnerId")]
		//public virtual Account Account { get; set; }

		public static implicit operator BoardInfo(Board board)
		{
			return new BoardInfo()
			{
				Id = board.Id,
				Name = board.Name,
				Owner = board.Owner,
				Priority = board.Priority,
				ColumnCount = board.ColumnCount,
				Created = board.Created,
				Updated = board.Updated,
				Status = board.Status,
				HashCode = board.HashCode
			};
		}
	}
}
