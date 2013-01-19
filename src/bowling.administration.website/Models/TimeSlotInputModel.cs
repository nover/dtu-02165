using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bowling.administration.website.Models
{
	public class TimeSlotInputModel
	{
		public int Id { get; set; }

		[Required]
		[DataType(DataType.Time)]
		public TimeSpan Start { get; set; }

		[Required]
		[DataType(DataType.Time)]
		public TimeSpan End { get; set; }
	}
}