using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class LaneSchedulerAction
    {
        public int leftmostLane;
        public int lowestTimeSlot;
        public int numLanes;
        public int numTimeSlots;
        public double weight;

        public LaneSchedulerReservation reservation;

        public LaneSchedulerAction(int leftmostLane, LaneSchedulerReservation reservation, double weight)
        {
            this.leftmostLane = leftmostLane;
            this.lowestTimeSlot = reservation.StartTimeSlot;
            this.reservation = reservation;
            this.weight = weight;
            this.numLanes = reservation.NumberOfLanes;
            this.numTimeSlots = reservation.NumberOfTimeSlots;
        }
    }
}
