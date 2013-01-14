using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bowling.Scheduling
{
    class Scheduler
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting");

            UnitTests.Test_n_reservations(20, 24, 50);
            Debug.WriteLine("Done");
            Console.Read();
        }

        public static State Search(State state, List<Reservation> reservations)
        {
            //Debug.WriteLine("Search is running. Number of reservations: " + reservations.Count);
            if (reservations.Count == 0)
            {
                return state;
            }
            // Get applicable action
            //Debug.WriteLine("Getting actions");
            List<Action> actions = GetActions(state, reservations);
            //Debug.WriteLine("Number of Actions: " + actions.Count);
            //Debug.WriteLine();
            // If there are no applicable actions, break and backtrack.
            if (actions.Count == 0 || state == null) {
                Debug.WriteLine("    Backtracking");
                return null;
            }
            actions = (from y in actions
                                 select y).OrderByDescending(y => y.weight).ToList<Action>();

            // Loop for actions in a depth-first manner, backtracking if no solution is found. 
            foreach (Action action in actions) {
                //Debug.WriteLine("    Trying action for reservation: " + action.reservation.id + " with weight: " + action.weight);
                State newState = state.Apply(action);

                List<Reservation> remainingReservations = new List<Reservation>(reservations);
                remainingReservations.Remove(action.reservation);
                State solution = Search(newState, remainingReservations);
                if (solution != null) {
                    return solution;
                }
            }
            Debug.WriteLine("    Backtracking");
            return null;
        }

        public static List<Action> GetActions(State state, List<Reservation> reservations)
        {
            List<Action> actions = new List<Action>();
            foreach (Reservation reservation in reservations) {
                //Debug.WriteLine("Expanding reservation " + reservation.id);
                foreach(Action action in Expand(state, reservation)) {
                    int toLane = action.leftmostLane + action.leftmostLane + action.numLanes;
                    //Debug.WriteLine("   Got an action on lane: " + action.leftmostLane + " to lane: " + toLane + " for at time: " + action.lowestTimeSlot);
                    actions.Add(action); // add at the end of list, so they are in order
                }
            }
            return actions;
        }

        public static List<Action> Expand(State state, Reservation reservation)
        {
            List<Action> actions = new List<Action>();
            for (int lane = 0; lane < state.numberOfLanes; lane++)
            {
                AppWeightPair appWeightPair = state.IsApplicable(lane, reservation.numLanes, reservation.numTimeSlots, reservation.startTimeSlot);
                if (appWeightPair.applicable)
                {
                    Action action = new Action(lane, reservation, appWeightPair.weight);
                    actions.Add(action);
                }
            }
            return actions;
        }
    }

    class Reservation {
        public int id;
        public int numLanes;
        public int numTimeSlots;
        public int startTimeSlot;

        public Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot) {
            this.id = id;
            this.numLanes = numLanes;
            this.numTimeSlots = numTimeSlots;
            this.startTimeSlot = startTimeSlot;
        }
    }

    class Action {
        public int leftmostLane;
        public int lowestTimeSlot;
        public int numLanes;
        public int numTimeSlots;
        public float weight;

        public Reservation reservation;

        public Action(int leftmostLane, Reservation reservation, float weight)
        {
            this.leftmostLane = leftmostLane;
            this.lowestTimeSlot = reservation.startTimeSlot;
            this.reservation = reservation;
            this.weight = weight;
            this.numLanes = reservation.numLanes;
            this.numTimeSlots = reservation.numTimeSlots;
        }
    }

    class State {
        int[,] state;
        public int numberOfLanes;
        public int numberOfTimeSlots;
        float[] weight;

        public State(int numberOfLanes, int numberOfTimeSlots, List<Reservation> reservations)
        {
            this.numberOfLanes = numberOfLanes;
            this.numberOfTimeSlots = numberOfTimeSlots;
            this.state = new int[numberOfTimeSlots, numberOfLanes];
            this.weight = new float[numberOfTimeSlots];

            foreach (Reservation reservation in reservations) {
                for (int i = reservation.startTimeSlot; i < reservation.startTimeSlot + reservation.numTimeSlots; i++) {
                    for (int j = 0; j < reservation.numLanes; j++)
                    {
                        this.weight[i] += 1.0f;
                    }
                }
                //Debug.WriteLine("Weight for timeslot: " + reservation.startTimeSlot + "  is now:  " + this.weight[reservation.startTimeSlot]);
            }
            string weightString = "[";
            for (int i = 0; i < this.weight.Length; i++) {
                weightString += " " + this.weight[i];
            }
            weightString += "]";
            //Debug.WriteLine("Weights are now:  " + weightString);
        }

        public State Apply(Action action)
        {
            for (int i = action.lowestTimeSlot; i < action.lowestTimeSlot + action.numTimeSlots; i++)
            {
                for (int j = action.leftmostLane; j < action.leftmostLane + action.numLanes; j++) {
                    this.state[i, j] = action.reservation.id;
                }
            }
            return this;
        }

        public AppWeightPair IsApplicable(int lane, int numLanes, int numTimeSlots, int startTimeSlot)
        {
            //Debug.WriteLine("Checking for a reservation for " + numLanes + " lanes at lane: " + lane + " at timeslot: " + startTimeSlot);
            float weight = 0.0f;
            for (int i = startTimeSlot; i < startTimeSlot + numTimeSlots; i++)
            {
                if (i >= this.numberOfTimeSlots)
                {
                    return new AppWeightPair(false, 0);
                }
                for (int j = lane; j < lane + numLanes; j++)
                {
                    if (j >= this.numberOfLanes || this.state[i, j] != 0)
                    {
                        return new AppWeightPair(false, 0);
                    }
                    else {
                        weight += this.getCellWeight(i);
                        //Debug.WriteLine("Weight updated with: " + this.getCellWeight(i) + " to: " + weight);
                    }
                }
            }
            //Debug.WriteLine("    Weight starts at: " + weight);
            // Various weight-changing rules for ensuring spread and guests sharing computers and ball-returns
            // Even numbered numLanes should be placed an even-numbered lane.
            if(numLanes % 2 == 0 && lane % 2 == 0) {
                weight += 1.0f;
            }
            // Take wear and tear into account. Use the reciprocal of the combined wear values for all used lanes, to ensure the combined lowest get the highest value.
            int wear = WearData.getWearForLanes(lane, numLanes);
            if(wear == 0) {
                wear = 1;
            }
            float wearValue = 1.0f / wear;
            weight += wearValue;

            // Try to spread reservations out
            // Get combined distance to other reservations
            float distance = this.getCombinedDistanceToOthers(lane, numLanes, startTimeSlot);
            // Favour those with most distance to others - but not too much.
            //Debug.WriteLine("    Weight before distance: " + weight + " distance was: " + distance);
            weight += distance / this.numberOfLanes;
            //Debug.WriteLine("    Weight ends at: " + weight);
            return new AppWeightPair(true, weight);
        }

        public float getCombinedDistanceToOthers(int lane, int numLanes, int startTimeSlot)
        {
            float distance = 0.0f;
            // get right distance
            for (int i = lane + 1; i < this.numberOfLanes; i++)
            {
                //Debug.WriteLine("    Checking pos right: " + startTimeSlot + "," + i + " with value: " + this.state[startTimeSlot, i]);
                if(i == this.numberOfLanes - 1) {
                    break;
                }
                if (this.state[startTimeSlot, i] != 0)
                {
                    distance += i - lane - 1;
                    //Debug.WriteLine("        and distance: " + i + " - " + lane + " = " + distance);
                    break;
                }
            }
            // get left distance
            for (int i = lane - 1; i >= 0; i--)
            {
                //Debug.WriteLine("    Checking pos left: " + startTimeSlot + "," + i + " with value: " + this.state[startTimeSlot, i] + " and distance: " + lane + " - " + i + " = " + distance);
                if (i == 0) {
                    break;
                }
                if (this.state[startTimeSlot, i] != 0)
                {
                    distance += lane - i - 1;
                    //Debug.WriteLine("        and distance: " + lane + " - " + i + " = " + distance);
                    break;
                }
            }
            return distance;
        }

        public float getCellWeight(int timeslot)
        {
            return this.weight[timeslot];
        }

        public string toString() {
            string repr = "";
            for (int i = this.numberOfTimeSlots-1; i >= 0; i--)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    string numRepr = "";
                    if (this.state[i, j] == 0)
                    {
                        numRepr = "..";
                    } else if (this.state[i, j] < 10)
                    {
                        numRepr = "0" + this.state[i, j];
                    }
                    else {
                        numRepr = "" + this.state[i, j];
                    }
                    repr = repr + "  " + numRepr;
                }
                repr = repr + "\n";
            }
            return repr;
        }
    }

    class AppWeightPair {
        public bool applicable;
        public float weight;

        public AppWeightPair(bool applicable, float weight)
        {
            this.applicable = applicable;
            this.weight = weight;
        }
    }


    static class WearData
    {
        private static List<int> wear = new List<int>();

        public static void Populate(int numLanes) {
            for (int i = 0; i < numLanes; i++) {
                Random random = new Random();
                int randomNumber = random.Next(0, 100);
                wear.Add(randomNumber);
            }
        }

        public static void countUpWearForLane(int lane)
        {
            wear[lane] =+ 1;
        }

        public static int getWearForLane(int lane)
        {
            return wear[lane];
        }

        public static int getWearForLanes(int lane, int numOfLanes)
        {
            int wearVal = 0;
            for (int i = lane; i < lane + numOfLanes; i++) {
                try
                {
                    wearVal += wear[i];
                }
                catch (System.ArgumentOutOfRangeException) {
                    // Do nothing
                }
            }
            return wearVal;
        }
    }


    class UnitTests
    {

        public static bool Test1_EmptyState()
        {
            int numberOfLanes = 6;
            int numberOfTimeSlots = 5;
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();

            State emptyState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            return true;
        }

        public static bool Test2_SmallReservation()
        {
            int numberOfLanes = 8;
            int numberOfTimeSlots = 5;
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            Reservation res1 = new Reservation(1, 2, 2, 3);
            Reservation res2 = new Reservation(2, 2, 2, 2);
            Reservation res3 = new Reservation(3, 1, 2, 2);

            // add reservations
            reservations.Add(res1);
            reservations.Add(res2);
            reservations.Add(res3);

            // Test Search
            State thirdState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State finalState = Program.Search(thirdState, reservations);
            Debug.WriteLine("A state with three reservations - planned by the scheduler");
            Debug.WriteLine(finalState.toString());
            // (int lane, int numLanes, int startTimeSlot)
            Debug.WriteLine("Distance from 6, 2, 3  :  " + finalState.getCombinedDistanceToOthers(6, 2, 3));
            
            if (finalState == null)
            {
                return false;
            }
            else {
                return true;
            }
        }

        public static bool Test3_FailingReservation()
        {
            int numberOfLanes = 6;
            int numberOfTimeSlots = 5;
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            Reservation res1 = new Reservation(1, 2, 2, 3);
            Reservation res2 = new Reservation(2, 2, 2, 2);
            Reservation res3 = new Reservation(3, 1, 2, 2);
            Reservation res4 = new Reservation(4, 6, 2, 3);
            
            // Test Search
            reservations.Add(res1);
            reservations.Add(res2);
            reservations.Add(res3);
            reservations.Add(res4);
            State fourthState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State failState = Program.Search(fourthState, reservations);

            Debug.WriteLine("A conflicting state, which should fail");
            if (failState == null)
            {
                Debug.WriteLine("Scheduling failed - test succeeded");
                return true;
            }
            else {
                Debug.WriteLine("Scheduling succeeded - something is wrong");
                Debug.WriteLine(failState.toString());
                return false;
            }
        }

        public static bool Test4_LargeReservation()
        {
            int numberOfLanes = 50;
            int numberOfTimeSlots = 24;
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            Reservation res1 = new Reservation(1, 2, 2, 5);
            Reservation res2 = new Reservation(2, 2, 2, 2);
            Reservation res3 = new Reservation(3, 1, 1, 7);
            Reservation res4 = new Reservation(4, 6, 3, 3);
            Reservation res5 = new Reservation(5, 2, 1, 3);
            Reservation res6 = new Reservation(6, 2, 2, 2);
            Reservation res7 = new Reservation(7, 2, 1, 1);
            Reservation res8 = new Reservation(8, 2, 2, 1);
            Reservation res9 = new Reservation(9, 3, 3, 3);
            Reservation res10 = new Reservation(10, 1, 3, 4);
            Reservation res11 = new Reservation(11, 1, 2, 3);
            Reservation res12 = new Reservation(12, 2, 2, 4);
            Reservation res13 = new Reservation(13, 3, 4, 5);
            Reservation res14 = new Reservation(14, 2, 2, 6);
            
            Reservation res15 = new Reservation(15, 1, 1, 8);
            Reservation res16 = new Reservation(16, 2, 1, 9);
            
            Reservation res17 = new Reservation(17, 3, 1, 9);
            Reservation res18 = new Reservation(18, 2, 2, 4);
            
            Reservation res19 = new Reservation(19, 2, 2, 1);
            Reservation res20 = new Reservation(20, 1, 2, 7);
            /*   */

            // Test Search
            reservations.Add(res1);
            reservations.Add(res2);
            reservations.Add(res3);
            reservations.Add(res4);
            reservations.Add(res5);
            reservations.Add(res6);
            reservations.Add(res7);
            reservations.Add(res8);
            reservations.Add(res9);
            reservations.Add(res10);
            reservations.Add(res11);
            reservations.Add(res12);
            reservations.Add(res13);
            reservations.Add(res14);
            reservations.Add(res15);
            reservations.Add(res16);
            
            reservations.Add(res17);
            reservations.Add(res18);
            
            reservations.Add(res19);
            reservations.Add(res20);
            /* */
            State fourthState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State largeState = Program.Search(fourthState, reservations);

            Debug.WriteLine("A large state - might take some time");
            if (largeState == null)
            {
                Debug.WriteLine("Scheduling failed - test failed");
                return false;
            }
            else
            {
                Debug.WriteLine("Scheduling succeeded - Test succeeded");
                Debug.WriteLine(largeState.toString());
                return true;
            }
        }

        public static bool Test_n_reservations(int numberOfLanes, int numberOfTimeSlots, int numberOfReservations)
        {
            Debug.WriteLine("Testing scheduling of " + numberOfReservations + " reservations in " + numberOfLanes + " lanes and " + numberOfTimeSlots + " timeslots");
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            bool run = true;
            State state = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State emptyState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            int i = 0;
            long timeSpent = 0;
            while (run) {
                Random random = new Random();
                int numLanes = random.Next(1, 3);
                int numTimeSlots = random.Next(1, 3);
                int startTimeSlot = random.Next(0, 23);
                long time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                Reservation reservation = new Reservation(i+1, numLanes, numTimeSlots, startTimeSlot);
                Debug.WriteLine("Making reservation at of " + numLanes + " for " + numTimeSlots + " hours, at timeslot " + startTimeSlot);
                List<Reservation> newReservations = new List<Reservation>(reservations);
                newReservations.Add(reservation);

                newState = Program.Search(emptyState, newReservations);
                long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                timeSpent = time2 - time1;
                Debug.WriteLine("    Scheduling took: " + timeSpent + " miliseconds");
                if (newState != null)
                {
                    Debug.WriteLine("    It succeeded");
                    reservations = newReservations;
                    i++;
                }
                else {
                    Debug.WriteLine("    It failed");
                }
                if (i > numberOfResevations) {
                    run = false;
                }
            }
            Debug.WriteLine("The last scheduling took: " + timeSpent + " miliseconds");
            Debug.WriteLine(state.toString());
            return true;
        }
    }

}