using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class LaneSchedulerStateReservationsPair {
        public LaneSchedulerState state;
        public List<LaneSchedulerReservation> reservations;

        public LaneSchedulerStateReservationsPair(LaneSchedulerState state, List<LaneSchedulerReservation> reservations) {
            this.state = state;
            this.reservations = reservations;
        }
    }
}
