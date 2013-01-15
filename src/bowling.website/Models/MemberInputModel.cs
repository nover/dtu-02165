using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bowling.Web.CustomerSite.Models
{
	public class MemberInputModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}