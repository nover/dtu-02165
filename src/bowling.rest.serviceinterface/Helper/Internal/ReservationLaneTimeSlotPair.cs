using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Helper.Internal
{
	class ReservationLaneTimeSlotPair
	{
		public int ReservationId { get; set; }

		public int LaneId { get; set; }

		public int TimeSlotId { get; set; }
	}
}
