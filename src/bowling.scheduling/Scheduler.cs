using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class Scheduler
    {
        static Dictionary<string, int> closedStateList;
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting");

            PerformanceTests.Test_n_reservations(20, 16, 700, 500);

            //UnitTests.Test4_ProblematicReservation();
            Debug.WriteLine("Done");
            Console.Read();
        }

        public static StateReservationsPair Search(State state, List<Reservation> reservations, Reservation newReservation)
        {
            Scheduler.closedStateList = new Dictionary<string, int>();
            Debug.WriteLine("Adding new Reservation");
            if (!state.isPossible(newReservation))
            {
                Debug.WriteLine("    It failed the first check");
                // get other reservations
                return new StateReservationsPair(null, Scheduler.getAlternativeReservations(state, newReservation));
            }
            // if it can be applied easily
            List<Action> actions = Expand(state, newReservation);
            actions = (from y in actions
                       select y).OrderBy(y => y.weight).ToList<Action>();
            if (actions.Count > 0)
            {
                state.Apply(actions[0]);
                Debug.WriteLine("    The reservation was straight forward");
                return new StateReservationsPair(state, new List<Reservation>());
            }

            // else, search

            // cut the relevant piece out of the state, so we end up with three pieces.
            // Solve the middle piece, and add it to the top and bottom piece.
            //List<State> statePieces = state.cutInPieces(newReservation);



            List<Reservation> newReservations = new List<Reservation>(reservations);
            newReservations.Add(newReservation);

            long time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            State emptyState = new State(state.numberOfLanes, state.numberOfTimeSlots, newReservations);
            State newState = Scheduler.RecursiveSearch(emptyState, newReservations, 0, 500, 0);

            long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long runTime = time2 - time1;
            Debug.WriteLine("    Search took: " + runTime + " ms");
            if (newState != null)
            {
                Debug.WriteLine("    Returning state");
                return new StateReservationsPair(newState, new List<Reservation>());
            }
            else
            {
                // get other reservations
                Debug.WriteLine("    Returning other reservations");
                return new StateReservationsPair(null, Scheduler.getAlternativeReservations(state, newReservation));
            }
        }

        public static State RecursiveSearch(State state, List<Reservation> reservations, int depth, long timelimit, long time)
        {
            /*if (depth > 30) {
                List<State> statePieces = state.cutInPieces(reservations[0]);
                Debug.WriteLine("State Pieces: ");
                foreach (State s in statePieces)
                {
                    Debug.WriteLine(s.toString());
                }
            }*/
            if (time > timelimit)
            {
                //Debug.WriteLine("TIME LIMIT EXCEEDED-1! Time was: " + time);
                return null;
            }

            long time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            //Debug.WriteLine("Reached depth: " + depth);
            if (reservations.Count == 0)
            {
                return state;
            }
            reservations = (from y in reservations
                            select y).OrderBy(y => state.getReservationWeight(y)).ToList<Reservation>();
            foreach (Reservation reservation in reservations)
            {
                // Get applicable actions
                List<Action> actions = Expand(state, reservation);

                actions = (from y in actions
                           select y).OrderBy(y => y.weight).ToList<Action>();

                // Loop for actions in a depth-first manner, backtracking if no solution is found. 
                for (int num = 0; num < actions.Count; num++)
                {
                    Action action = actions[num];
                    state.Apply(action);

                    //if (!Scheduler.closedStateList.ContainsKey(state.ReservationRepr(action.reservation)))
                    //{
                    List<Reservation> remainingReservations = new List<Reservation>(reservations);
                    remainingReservations.Remove(action.reservation);
                    int newDepth = depth + 1;
                    long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    long runTime = time2 - time1;
                    time = time + runTime;
                    State solution = RecursiveSearch(state, remainingReservations, newDepth, timelimit, time);
                    if (solution != null)
                    {
                        //Debug.WriteLine("Placed reservation: " + action.reservation.id);
                        return solution;
                    }
                    else
                    {
                        //string stateRepr = state.ReservationRepr(action.reservation);
                        //if (!Scheduler.closedStateList.ContainsKey(stateRepr))
                        //{
                        //    Scheduler.closedStateList.Add(stateRepr, 1);
                        //}
                        state.Unapply(action);
                        if (time > timelimit)
                        {
                            //Debug.WriteLine("TIME LIMIT EXCEEDED-2! Time was: " + time);
                            return null;
                        }
                        //Debug.WriteLine(state.toString());
                    }
                    //}else{
                    //Debug.WriteLine("    Hit an explored state!");
                    //Debug.WriteLine(state.toString());
                    //Debug.WriteLine("    " + state.ReservationRepr(action.reservation));
                    //state.Unapply(action);
                    //return null;
                    //}
                }
                //Debug.WriteLine("    Backtracking");
                //Debug.WriteLine(state.toString());
            }
            return null;
        }

        public static List<Action> GetActions(State state, List<Reservation> reservations)
        {
            List<Action> actions = new List<Action>();
            foreach (Reservation reservation in reservations)
            {
                foreach (Action action in Expand(state, reservation))
                {
                    actions.Add(action); // add at the end of list, so they are in order
                }
            }
            return actions;
        }

        public static List<Action> Expand(State state, Reservation reservation)
        {
            List<Action> actions = new List<Action>();
            if (state.isPossible(reservation))
            {
                for (int lane = 0; lane < state.numberOfLanes; lane++)
                {
                    AppWeightPair appWeightPair = state.IsApplicable(lane, reservation.numLanes, reservation.numTimeSlots, reservation.startTimeSlot);
                    if (appWeightPair.applicable)
                    {
                        actions.Add(new Action(lane, reservation, appWeightPair.weight));
                    }
                }
            }
            return actions;
        }

        public static List<Reservation> getAlternativeReservations(State state, Reservation reservation)
        {
            List<Reservation> reservations = new List<Reservation>();

            if (reservation.startTimeSlot - 1 > 0)
            {
                Reservation altReservation1 = new Reservation(reservation.id, reservation.numLanes, reservation.numTimeSlots, reservation.startTimeSlot - 1);
                List<Action> actions = Scheduler.Expand(state, altReservation1);
                if (actions.Count > 0)
                {
                    reservations.Add(altReservation1);
                }
                int numTimeSlots = reservation.numTimeSlots - 1;
                while (numTimeSlots > 0)
                {
                    Reservation altReservation2 = new Reservation(reservation.id, reservation.numLanes, numTimeSlots, reservation.startTimeSlot - 1);
                    List<Action> actions2 = Scheduler.Expand(state, altReservation2);
                    if (actions2.Count > 0)
                    {
                        reservations.Add(altReservation2);
                    }
                    numTimeSlots--;
                }
            }

            if (reservation.startTimeSlot + 1 < state.numberOfTimeSlots)
            {
                Reservation altReservation1 = new Reservation(reservation.id, reservation.numLanes, reservation.numTimeSlots, reservation.startTimeSlot + 1);
                List<Action> actions = Scheduler.Expand(state, altReservation1);
                if (actions.Count > 0)
                {
                    reservations.Add(altReservation1);
                }
                int numTimeSlots = reservation.numTimeSlots - 1;
                while (numTimeSlots > 0)
                {
                    Reservation altReservation2 = new Reservation(reservation.id, reservation.numLanes, numTimeSlots, reservation.startTimeSlot + 1);
                    List<Action> actions2 = Scheduler.Expand(state, altReservation2);
                    if (actions2.Count > 0)
                    {
                        reservations.Add(altReservation2);
                    }
                    numTimeSlots--;
                }
            }

            return reservations;
        }
    }
}
