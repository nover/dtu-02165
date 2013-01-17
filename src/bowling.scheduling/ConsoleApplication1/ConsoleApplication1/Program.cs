using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bowling.Scheduling
{
    class StateReservationsPair {
        public State state;
        public List<Reservation> reservations;

        public StateReservationsPair(State state, List<Reservation> reservations) {
            this.state = state;
            this.reservations = reservations;
        }
    }

    class Scheduler
    {
        static Dictionary<string, int> closedStateList;
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting");

            PerformanceTests.Test_n_reservations(10, 16, 40, 500);

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
            if (actions.Count > 0) {
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
            State newState = Scheduler.RecursiveSearch(emptyState, newReservations, 0, 10000000, 0);

            long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long runTime = time2 - time1;
            Debug.WriteLine("    Search took: " + runTime + " ms");
            if (newState != null)
            {
                Debug.WriteLine("    Returning state");
                return new StateReservationsPair(newState, new List<Reservation>());
            }
            else {
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
            if (time > timelimit) {
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
                        string stateRepr = state.ReservationRepr(action.reservation);
                        //if (!Scheduler.closedStateList.ContainsKey(stateRepr))
                        //{
                        //Scheduler.closedStateList.Add(stateRepr, 1);
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
                    //    state.Unapply(action);
                    //    return null;
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
                while (numTimeSlots > 0) {
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

    class Reservation
    {
        public int id;
        public int numLanes;
        public int numTimeSlots;
        public int startTimeSlot;

        public Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
        {
            this.id = id;
            this.numLanes = numLanes;
            this.numTimeSlots = numTimeSlots;
            this.startTimeSlot = startTimeSlot;
        }
    }

    class Action
    {
        public int leftmostLane;
        public int lowestTimeSlot;
        public int numLanes;
        public int numTimeSlots;
        public double weight;

        public Reservation reservation;

        public Action(int leftmostLane, Reservation reservation, double weight)
        {
            this.leftmostLane = leftmostLane;
            this.lowestTimeSlot = reservation.startTimeSlot;
            this.reservation = reservation;
            this.weight = weight;
            this.numLanes = reservation.numLanes;
            this.numTimeSlots = reservation.numTimeSlots;
        }
    }

    class State
    {
        int[,] state;
        public int numberOfLanes;
        public int numberOfTimeSlots;
        double[] weight;

        public State(int numberOfLanes, int numberOfTimeSlots, List<Reservation> reservations)
        {
            this.numberOfLanes = numberOfLanes;
            this.numberOfTimeSlots = numberOfTimeSlots;
            this.state = new int[numberOfTimeSlots, numberOfLanes];
            this.weight = new double[numberOfTimeSlots];

            foreach (Reservation reservation in reservations)
            {
                for (int i = reservation.startTimeSlot; i < reservation.startTimeSlot + reservation.numTimeSlots; i++)
                {
                    for (int j = 0; j < reservation.numLanes; j++)
                    {
                        if (i < numberOfTimeSlots)
                        {
                            this.weight[i] += 1.0f;
                        }
                    }
                }
            }
        }

        public void Apply(Action action)
        {
            for (int i = action.lowestTimeSlot; i < action.lowestTimeSlot + action.numTimeSlots; i++)
            {
                for (int j = action.leftmostLane; j < action.leftmostLane + action.numLanes; j++)
                {
                    if (i < this.numberOfTimeSlots && j < this.numberOfLanes)
                    {
                        this.state[i, j] = action.reservation.id;
                    }
                }
            }
        }

        public void Unapply(Action action)
        {
            for (int i = action.lowestTimeSlot; i < action.lowestTimeSlot + action.numTimeSlots; i++)
            {
                for (int j = action.leftmostLane; j < action.leftmostLane + action.numLanes; j++)
                {
                    if (i < this.numberOfTimeSlots && j < this.numberOfLanes)
                    {
                        this.state[i, j] = 0;
                    }
                }
            }
        }

        public bool isPossible(Reservation reservation)
        {
            for (int i = reservation.startTimeSlot; i < reservation.numTimeSlots + reservation.startTimeSlot; i++) {
                if (i >= this.numberOfTimeSlots) {
                    return false;
                }
                int numFreeCells = 0;
                for (int j = 0; j < this.numberOfLanes; j++) {
                    if (this.state[i, j] == 0) {
                        numFreeCells++;
                    }
                }
                if (numFreeCells < reservation.numLanes) {
                    return false;
                }
            }
            return true;
        }

        public AppWeightPair IsApplicable(int lane, int numLanes, int numTimeSlots, int startTimeSlot)
        {
            //Debug.WriteLine("Checking for a reservation for " + numLanes + " lanes at lane: " + lane + " at timeslot: " + startTimeSlot);
            double weight = 0.0f;
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
                    else
                    {
                        //weight = weight + this.getCellWeight(i);
                        //Debug.WriteLine("Weight updated with: " + this.getCellWeight(i) + " to: " + weight);
                    }
                }
                weight = Math.Max(weight, this.getCellWeight(i));
            }
            weight += 1 / numLanes;
            //Debug.WriteLine("    Weight starts at: " + weight);
            // Various weight-changing rules for ensuring spread and guests sharing computers and ball-returns
            // Even numbered numLanes should be placed an even-numbered lane.
            if (numLanes % 2 == 0 && lane % 2 == 0)
            {
                weight += 1.0f;
            }
            // Take wear and tear into account. Use the reciprocal of the combined wear values for all used lanes, to ensure the combined lowest get the highest value.
            int wear = WearData.getWearForLanes(lane, numLanes);
            if (wear == 0)
            {
                wear = 1;
            }
            double wearValue = 1.0f / wear;
            weight += wearValue;

            // Try to spread reservations out
            // Get combined distance to other reservations
            double distance = this.getCombinedDistanceToOthers(lane, numLanes, startTimeSlot);
            // Favour those with most distance to others - but not too much.
            //Debug.WriteLine("    Weight before distance: " + weight + " distance was: " + distance);

            weight += 1 / (distance);
            //Debug.WriteLine("    Weight ends at: " + weight);
            return new AppWeightPair(true, weight);
        }

        public double getCombinedDistanceToOthers(int lane, int numLanes, int startTimeSlot)
        {
            double distanceRight = 0.0f;
            double distanceLeft = 0.0f;
            // get right distance
            for (int i = lane + 1; i < this.numberOfLanes; i++)
            {
                //Debug.WriteLine("    Checking pos right: " + startTimeSlot + "," + i + " with value: " + this.state[startTimeSlot, i]);
                if (i == this.numberOfLanes - 1)
                {
                    distanceRight = this.numberOfLanes;
                    break;
                }
                if (this.state[startTimeSlot, i] != 0)
                {
                    distanceRight = i - lane - 1;
                    //Debug.WriteLine("        and distance-right: " + i + " - " + lane + " = " + distanceRight);
                    break;
                }
            }
            // get left distance
            for (int i = lane - 1; i >= 0; i--)
            {
                //Debug.WriteLine("    Checking pos left: " + startTimeSlot + "," + i + " with value: " + this.state[startTimeSlot, i]);
                if (i <= 0)
                {
                    break;
                }
                if (this.state[startTimeSlot, i] != 0)
                {
                    distanceLeft = lane - i - 1;
                    //Debug.WriteLine("        and distance-left: " + lane + " - " + i + " = " + distanceLeft);
                    break;
                }
            }

            double distance = Math.Min(distanceRight, distanceLeft);
            //Debug.WriteLine("        Returning distance " + distanceRight + " <min> " + distanceLeft + "  -> " + distance);
            return distance;
        }

        public double getCellWeight(int timeslot)
        {
            return this.weight[timeslot];
        }

        public string toString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = this.numberOfTimeSlots - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    string numRepr;
                    if (this.state[i, j] == 0)
                    {
                        numRepr = "..";
                    }
                    else if (this.state[i, j] < 10)
                    {
                        numRepr = "0" + this.state[i, j];
                        //numRepr = "" + this.getCellWeight(i);
                    }
                    else
                    {
                        numRepr = "" + this.state[i, j];
                        //numRepr = "" + this.getCellWeight(i);
                    }
                    
                    builder.Append("  " + numRepr);
                }
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public double getReservationWeight(Reservation reservation) {
            double weight = 0.0f;

            for (int i = reservation.startTimeSlot; i < reservation.startTimeSlot + reservation.numTimeSlots; i++) {
                weight = Math.Max(weight, this.getCellWeight(i));
            }

            return weight;
        }

        public String Repr()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.numberOfTimeSlots; i++)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    builder.Append(this.state[i, j]);
                }
            }
            return builder.ToString();
        }

        public String ReservationRepr(Reservation reservation)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(reservation.id);
            builder.Append("1");
            builder.Append(reservation.numLanes);
            builder.Append("1");
            builder.Append(reservation.numTimeSlots);
            builder.Append("1");
            builder.Append(reservation.startTimeSlot);
            builder.Append("1");
            for (int i = reservation.startTimeSlot - reservation.numTimeSlots; i < reservation.startTimeSlot + reservation.numTimeSlots; i++)
            {
                if (i < this.numberOfTimeSlots && i >= 0)
                {
                    for (int j = 0; j < this.numberOfLanes; j++)
                    {
                        builder.Append(this.state[i, j]);
                    }
                    builder.Append("/");
                }
            }
            return builder.ToString();
        }

        public List<State> cutInPieces(Reservation reservation)
        {
            List<State> stateList = new List<State>();

            // detect cutting lines
            // find cutting line for upper part
            int upperCut = -1;
            for (int i = reservation.startTimeSlot + reservation.numTimeSlots; i < this.numberOfTimeSlots; i++)
            {
                bool foundAny = false;
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    if (this.state[i, j] == this.state[i + 1, j] || this.state[i, j] == this.state[i - 1, j])
                    {
                        foundAny = true;
                    }
                    if (!foundAny)
                    {
                        upperCut = i;
                        break;
                    }
                }
            }
            // find cutting line for lower part
            int lowerCut = -1;
            for (int i = reservation.startTimeSlot - 1; i >= 0; i--)
            {
                bool foundAny = false;
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    if (this.state[i, j] == this.state[i + 1, j] || this.state[i, j] == this.state[i - 1, j])
                    {
                        foundAny = true;
                    }
                    if (!foundAny)
                    {
                        lowerCut = i;
                        break;
                    }
                }
            }

            // do the cutting
            State stateUpper = new State(this.numberOfLanes, this.numberOfTimeSlots, new List<Reservation>());
            stateUpper.weight = this.weight;
            stateUpper.weight = this.weight;
            stateUpper.state = new int[numberOfTimeSlots, numberOfLanes];

            State stateMiddle = new State(this.numberOfLanes, this.numberOfTimeSlots, new List<Reservation>());
            stateMiddle.weight = this.weight;
            stateMiddle.weight = this.weight;
            stateMiddle.state = new int[numberOfTimeSlots, numberOfLanes];

            State stateLower = new State(this.numberOfLanes, this.numberOfTimeSlots, new List<Reservation>());
            stateLower.weight = this.weight;
            stateLower.weight = this.weight;
            stateLower.state = new int[numberOfTimeSlots, numberOfLanes];

            // Populate them

            for (int i = 0; i < this.numberOfTimeSlots; i++)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    if (i <= lowerCut)
                    {
                        stateLower.state[i, j] = this.state[i, j];
                    }
                    else if (j >= upperCut)
                    {
                        stateMiddle.state[i, j] = this.state[i, j];
                    }
                    else
                    {
                        stateUpper.state[i, j] = this.state[i, j];
                    }
                }
            }
            stateList.Add(stateUpper);
            stateList.Add(stateMiddle);
            stateList.Add(stateLower);
            return stateList;
        }
    }

    class AppWeightPair
    {
        public bool applicable;
        public double weight;

        public AppWeightPair(bool applicable, double weight)
        {
            this.applicable = applicable;
            this.weight = weight;
        }
    }


    static class WearData
    {
        private static List<int> wear = new List<int>();

        public static void Populate(int numLanes)
        {
            for (int i = 0; i < numLanes; i++)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100);
                wear.Add(randomNumber);
            }
        }

        public static void countUpWearForLane(int lane)
        {
            wear[lane] = +1;
        }

        public static int getWearForLane(int lane)
        {
            return wear[lane];
        }

        public static int getWearForLanes(int lane, int numOfLanes)
        {
            int wearVal = 0;
            for (int i = lane; i < lane + numOfLanes; i++)
            {
                try
                {
                    wearVal += wear[i];
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    // Do nothing
                }
            }
            return wearVal;
        }
    }


    class UnitTests
    {
        public static bool Test1_SmallReservation()
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
            State finalState = Scheduler.RecursiveSearch(thirdState, reservations, 0, 0, 0);
            Debug.WriteLine(finalState.toString());

            if (finalState == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Test2_FailingReservation()
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
            State failState = Scheduler.RecursiveSearch(fourthState, reservations, 0, 0, 0);

            if (failState == null)
            {
                Debug.WriteLine("Scheduling failed - test succeeded");
                return true;
            }
            else
            {
                Debug.WriteLine("Scheduling succeeded - something is wrong");
                Debug.WriteLine(failState.toString());
                return false;
            }
        }

        public static bool Test3_OptimalPlacement()
        {
            int numberOfLanes = 6;
            int numberOfTimeSlots = 6;
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            Reservation res1 = new Reservation(1, 3, 2, 2);
            Reservation res2 = new Reservation(2, 3, 2, 2);
            
            // Test Search
            reservations.Add(res1);
            reservations.Add(res2);
           
            State emptyState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State state = Scheduler.RecursiveSearch(emptyState, reservations, 0, 0, 0);

            if (state == null)
            {
                Debug.WriteLine("Scheduling failed - test failed");
                return false;
            }
            else
            {
                Debug.WriteLine("Scheduling succeeded - Test succeeded");
                Debug.WriteLine(state.toString());
                return true;
            }
        }

        public static bool Test4_ProblematicReservation()
        {
            int numberOfLanes = 6;
            int numberOfTimeSlots = 8;
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            Reservation res1 = new Reservation(1, 3, 2, 0);
            Reservation res2 = new Reservation(2, 3, 2, 1);
            Reservation res3 = new Reservation(3, 1, 4, 2);
            Reservation res4 = new Reservation(4, 5, 2, 5);
            Reservation res5 = new Reservation(5, 2, 2, 2);

            // Test Search
            reservations.Add(res1);
            reservations.Add(res2);
            reservations.Add(res3);
            reservations.Add(res4);
            reservations.Add(res5);

            State emptyState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State state = Scheduler.RecursiveSearch(emptyState, reservations, 0, 0, 0);

            if (state == null)
            {
                Debug.WriteLine("Scheduling failed - test failed");
                return false;
            }
            else
            {
                Debug.WriteLine("Scheduling succeeded - Test succeeded");
                Debug.WriteLine(state.toString());
                return true;
            }
        }
    }


    class PerformanceTests
    {
        public static bool Test_n_reservations(int numberOfLanes, int numberOfTimeSlots, int numberOfReservations, int runLimit)
        {
            Debug.WriteLine("Testing scheduling of " + numberOfReservations + " reservations in " + numberOfLanes + " lanes and " + numberOfTimeSlots + " timeslots");
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            bool run = true;
            //State emptyState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State state = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State newState = null;
            int i = 0;
            int runs = 0;
            long timeSpent = 0;
            Random random = new Random();
            while (run)
            {
                int numLanes = 1;
                int numTimeSlots = random.Next(1, 3);
                int startTimeSlot = 6; //  random.Next(0, numberOfTimeSlots);
                if(random.Next(0, 100) < 10) { // 10 percent will be parties and outings
                    numLanes = random.Next(2, 5);
                }else{
                    numLanes = random.Next(1, 3);
                }

                if (random.Next(0, 100) < 15)
                {
                    startTimeSlot = random.Next(0, numberOfTimeSlots);
                }
                else
                {
                    startTimeSlot = random.Next(2, numberOfTimeSlots-2);
                }
                
                long time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                int id = i + 1;
                Reservation reservation = new Reservation(id, numLanes, numTimeSlots, startTimeSlot);
                Debug.WriteLine("Making reservation id: " + id + " of " + numLanes + " lanes for " + numTimeSlots + " hours, at timeslot " + startTimeSlot);

                List<Reservation> newReservations = new List<Reservation>(reservations);
                newReservations.Add(reservation);
                //emptyState = new State(numberOfLanes, numberOfTimeSlots, newReservations);
                StateReservationsPair result = Scheduler.Search(state, reservations, reservation);
                newState = result.state;

                long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                timeSpent = time2 - time1;
                Debug.WriteLine("    Scheduling took: " + timeSpent + " miliseconds");
                if (newState != null)
                {
                    Debug.WriteLine("    It SUCCEEDED!!!!!!!!");
                    Debug.WriteLine(newState.toString());

                    reservations = newReservations;
                    state = newState;
                    i++;
                }
                else
                {
                    Debug.WriteLine("    It failed. Alternative reservations are:");
                    for (int j = 0; j < result.reservations.Count; j++) {
                        Debug.WriteLine("        Reservation for " + result.reservations[j].numLanes + " lanes at slot: " + result.reservations[j].startTimeSlot + " for " + result.reservations[j].numTimeSlots + " hours");
                    }
                }
                if (i > numberOfReservations)
                {
                    run = false;
                }
                if (runs > runLimit) {
                    run = false;
                }
                runs++;
            }
            Debug.WriteLine("The last scheduling took: " + timeSpent + " miliseconds");
            if (state != null)
            {
                Debug.WriteLine(state.toString());
            }
            return true;
        }
    }

}