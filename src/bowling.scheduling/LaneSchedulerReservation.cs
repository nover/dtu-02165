using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class LaneSchedulerReservation
    {
        public int Id {get; set;}
		public int NumberOfLanes { get; set; }
		public int NumberOfTimeSlots { get; set; }
		public int StartTimeSlot { get; set; }

		public LaneSchedulerReservation()
		{

		}
        public LaneSchedulerReservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
        {
            this.Id = id;
            this.NumberOfLanes = numLanes;
            this.NumberOfTimeSlots = numTimeSlots;
            this.StartTimeSlot = startTimeSlot;
        }
    }
}
