using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class StateReservationsPair {
        public State state;
        public List<Reservation> reservations;

        public StateReservationsPair(State state, List<Reservation> reservations) {
            this.state = state;
            this.reservations = reservations;
        }
    }
}
