using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class LaneScheduler
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

        public static LaneSchedulerStateReservationsPair Search(LaneSchedulerState state, List<LaneSchedulerReservation> reservations, LaneSchedulerReservation newReservation)
        {
            LaneScheduler.closedStateList = new Dictionary<string, int>();
            Debug.WriteLine("Adding new Reservation");
            if (!state.IsPossible(newReservation))
            {
                Debug.WriteLine("    It failed the first check");
                // get other reservations
                return new LaneSchedulerStateReservationsPair(null, LaneScheduler.GetAlternativeReservations(state, newReservation));
            }
            // if it can be applied easily
            List<LaneSchedulerAction> actions = Expand(state, newReservation);
            actions = (from y in actions
                       select y).OrderBy(y => y.weight).ToList<LaneSchedulerAction>();
            if (actions.Count > 0)
            {
                state.Apply(actions[0]);
                Debug.WriteLine("    The reservation was straight forward");
                return new LaneSchedulerStateReservationsPair(state, new List<LaneSchedulerReservation>());
            }

            // else, search

            // cut the relevant piece out of the state, so we end up with three pieces.
            // Solve the middle piece, and add it to the top and bottom piece.
            //List<State> statePieces = state.cutInPieces(newReservation);



            List<LaneSchedulerReservation> newReservations = new List<LaneSchedulerReservation>(reservations);
            newReservations.Add(newReservation);

            long time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            LaneSchedulerState emptyState = new LaneSchedulerState(state.numberOfLanes, state.numberOfTimeSlots, newReservations);
            LaneSchedulerState newState = LaneScheduler.RecursiveSearch(emptyState, newReservations, 0, 100000, 0);

            long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long runTime = time2 - time1;
            Debug.WriteLine("    Search took: " + runTime + " ms");
            if (newState != null)
            {
                Debug.WriteLine("    Returning state");
                return new LaneSchedulerStateReservationsPair(newState, new List<LaneSchedulerReservation>());
            }
            else
            {
                // get other reservations
                Debug.WriteLine("    Returning other reservations");
                return new LaneSchedulerStateReservationsPair(null, LaneScheduler.GetAlternativeReservations(state, newReservation));
            }
        }

        public static LaneSchedulerState RecursiveSearch(LaneSchedulerState state, List<LaneSchedulerReservation> reservations, int depth, long timelimit, long time)
        {
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
                            select y).OrderBy(y => state.GetReservationWeight(y)).ToList<LaneSchedulerReservation>();
            foreach (LaneSchedulerReservation reservation in reservations)
            {
                // Get applicable actions
                List<LaneSchedulerAction> actions = Expand(state, reservation);

                actions = (from y in actions
                           select y).OrderBy(y => y.weight).ToList<LaneSchedulerAction>();

                // Loop for actions in a depth-first manner, backtracking if no solution is found. 
                for (int num = 0; num < actions.Count; num++)
                {
                    LaneSchedulerAction action = actions[num];
                    state.Apply(action);

                    //if (!Scheduler.closedStateList.ContainsKey(state.ReservationRepr(action.reservation)))
                    //{
                    List<LaneSchedulerReservation> remainingReservations = new List<LaneSchedulerReservation>(reservations);
                    remainingReservations.Remove(action.reservation);
                    int newDepth = depth + 1;
                    long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    long runTime = time2 - time1;
                    time = time + runTime;
                    LaneSchedulerState solution = RecursiveSearch(state, remainingReservations, newDepth, timelimit, time);
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

        public static List<LaneSchedulerAction> GetActions(LaneSchedulerState state, List<LaneSchedulerReservation> reservations)
        {
            List<LaneSchedulerAction> actions = new List<LaneSchedulerAction>();
            foreach (LaneSchedulerReservation reservation in reservations)
            {
                foreach (LaneSchedulerAction action in Expand(state, reservation))
                {
                    actions.Add(action); // add at the end of list, so they are in order
                }
            }
            return actions;
        }

        public static List<LaneSchedulerAction> Expand(LaneSchedulerState state, LaneSchedulerReservation reservation)
        {
            List<LaneSchedulerAction> actions = new List<LaneSchedulerAction>();
            if (state.IsPossible(reservation))
            {
                for (int lane = 0; lane < state.numberOfLanes; lane++)
                {
                    AppWeightPair appWeightPair = state.IsApplicable(lane, reservation.NumberOfLanes, reservation.NumberOfTimeSlots, reservation.StartTimeSlot);
                    if (appWeightPair.applicable)
                    {
                        actions.Add(new LaneSchedulerAction(lane, reservation, appWeightPair.weight));
                    }
                }
            }
            return actions;
        }

        public static List<LaneSchedulerReservation> GetAlternativeReservations(LaneSchedulerState state, LaneSchedulerReservation reservation)
        {
            List<LaneSchedulerReservation> reservations = new List<LaneSchedulerReservation>();

            if (reservation.StartTimeSlot - 1 > 0)
            {
                LaneSchedulerReservation altReservation1 = new LaneSchedulerReservation(reservation.Id, reservation.NumberOfLanes, reservation.NumberOfTimeSlots, reservation.StartTimeSlot - 1);
                List<LaneSchedulerAction> actions = LaneScheduler.Expand(state, altReservation1);
                if (actions.Count > 0)
                {
                    reservations.Add(altReservation1);
                }
                int numTimeSlots = reservation.NumberOfTimeSlots - 1;
                while (numTimeSlots > 0)
                {
                    LaneSchedulerReservation altReservation2 = new LaneSchedulerReservation(reservation.Id, reservation.NumberOfLanes, numTimeSlots, reservation.StartTimeSlot - 1);
                    List<LaneSchedulerAction> actions2 = LaneScheduler.Expand(state, altReservation2);
                    if (actions2.Count > 0)
                    {
                        reservations.Add(altReservation2);
                    }
                    numTimeSlots--;
                }
            }

            if (reservation.StartTimeSlot + 1 < state.numberOfTimeSlots)
            {
                LaneSchedulerReservation altReservation1 = new LaneSchedulerReservation(reservation.Id, reservation.NumberOfLanes, reservation.NumberOfTimeSlots, reservation.StartTimeSlot + 1);
                List<LaneSchedulerAction> actions = LaneScheduler.Expand(state, altReservation1);
                if (actions.Count > 0)
                {
                    reservations.Add(altReservation1);
                }
                int numTimeSlots = reservation.NumberOfTimeSlots - 1;
                while (numTimeSlots > 0)
                {
                    LaneSchedulerReservation altReservation2 = new LaneSchedulerReservation(reservation.Id, reservation.NumberOfLanes, numTimeSlots, reservation.StartTimeSlot + 1);
                    List<LaneSchedulerAction> actions2 = LaneScheduler.Expand(state, altReservation2);
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
