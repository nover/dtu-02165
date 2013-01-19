using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bowling.Web.CustomerSite.Models
{
	public class ReservationContactInfoInputModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.PhoneNumber)]
		public string Cellphone { get; set; }

		
	}
}