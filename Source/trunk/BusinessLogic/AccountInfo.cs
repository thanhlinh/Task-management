using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	[DataContract()]
	public class AccountInfo
	{
		public AccountInfo()
		{

		}

		[DataMember(Name = "id", IsRequired = true)]
		public int Id { get; set; }

		[DataMember(Name = "firstname", IsRequired = true)]
		public string FirtsName { get; set; }

		[DataMember(Name = "lastname", IsRequired = true)]
		public string LastName { get; set; }

		[DataMember(Name = "email", IsRequired = true)]
		public string Email { get; set; }

		[DataMember(Name = "type", IsRequired = true)]
		public AccountType AccountType { get; set; }

		[DataMember(Name = "code")]
		public Guid Code { get; set; }

		[DataMember(Name = "avatar")]
		public string Avatar { get; set; }

		[DataMember(Name = "role", IsRequired = true)]
		public string Role { get; set; }

		[DataMember(Name = "created", IsRequired = true)]
		public DateTime Created { get; set; }

		[DataMember(Name = "updated", IsRequired = true)]
		public DateTime Updated { get; set; }

		[DataMember(Name = "db", IsRequired = true)]
		public string DatabaseName
		{
			get
			{
				MD5 md5 = MD5.Create();
				Guid hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(this.Email)));
				return hash.ToString("N");
			}
		}
	}
}
