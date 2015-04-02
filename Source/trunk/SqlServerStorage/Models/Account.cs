using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

		[Required]
        public string FirstName { get; set; }

		[Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }


        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        public Guid Secret { get; set; }

        [Required]
        public Guid Code { get; set; }

        public string Avatar { get; set; }

		public bool IsExternal { get; set; }

		[Required]
		public string Role { get; set; }

		[Required]
		public DateTime Created { get; set; }

		[Required]
		public DateTime Updated { get; set; }


		[Required]
		public Status Status { get; set; }

        [NotMapped]
        public string Name
        {
            get { return FirstName + ' ' + LastName; }
        }

		public static implicit operator AccountInfo(Account account)
		{
			return new AccountInfo()
			{
				Id = account.Id,
				FirtsName = account.FirstName,
				LastName = account.LastName,
				Email = account.Email,
				AccountType = account.AccountType,
				Code = account.Code,
				Avatar = account.Avatar,
				Created = account.Created,
				Updated = account.Updated,
				Role = account.Role
			};
		}
    }
}
