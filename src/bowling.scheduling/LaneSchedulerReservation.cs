using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class LaneSchedulerReservation
    {
        public int id;
        public int numLanes;
        public int numTimeSlots;
        public int startTimeSlot;

        public LaneSchedulerReservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
        {
            this.id = id;
            this.numLanes = numLanes;
            this.numTimeSlots = numTimeSlots;
            this.startTimeSlot = startTimeSlot;
        }
    }
}
