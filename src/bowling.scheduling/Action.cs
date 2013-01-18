using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class Action
    {
        public int leftmostLane;
        public int lowestTimeSlot;
        public int numLanes;
        public int numTimeSlots;
        public double weight;

        public LaneSchedulerReservation reservation;

        public Action(int leftmostLane, LaneSchedulerReservation reservation, double weight)
        {
            this.leftmostLane = leftmostLane;
            this.lowestTimeSlot = reservation.startTimeSlot;
            this.reservation = reservation;
            this.weight = weight;
            this.numLanes = reservation.numLanes;
            this.numTimeSlots = reservation.numTimeSlots;
        }
    }
}
