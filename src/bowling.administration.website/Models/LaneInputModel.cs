using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bowling.administration.website.Models
{
	public class LaneInputModel
	{
		public int Id { get; set;}

		[Required]
		public string Name { get; set; }
	}
}