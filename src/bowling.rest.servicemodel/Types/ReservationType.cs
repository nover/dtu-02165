﻿using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Types
{
	[Route("/reservation/possible")]
	public class ReservationType
	{
		public int Id { get; set; }
		public DateTime PlayAt { get; set; }
		public TimeSpan TimeOfDay {get; set;}
		public int NumberOfPlayers { get; set; }
		public int HowManyHours { get; set; }
		public string Name { get; set; }
		public int PhoneNumber { get; set; }
		public int? MemberId { get; set; }
        public ReservationStatusType Status {get; set;}

		public IList<TimeSlotType> TimeSlots { get; set; }

		public IList<LaneType> Lanes { get; set; }

		public ReservationType ()
		{
			this.TimeSlots = new List<TimeSlotType>();
			this.Lanes = new List<LaneType>();
		}
	}
}
