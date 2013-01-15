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
        static Dictionary<string, string> closedStateList = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting");

            PerformanceTests.Test_n_reservations(10, 16, 40, 500);

            //UnitTests.Test4_ProblematicReservation();
            Debug.WriteLine("Done");
            Console.Read();
        }
        /*
        public static State Search(State state, List<Reservation> reservations, int depth)
        {
            //if(closedStateList.ContainsKey(state.Repr())) {
            //    Debug.WriteLine("    Backtracking-0");
            //    return null;
            //}

            Debug.WriteLine("Reached depth: " + depth);
            if (reservations.Count == 0 || state == null)
            {
                return state;
            }
            // Get applicable actions
            
            reservations = (from y in reservations
                       select y).OrderByDescending(y => y.weight).ToList<Reservation>();
            // Loop for actions in a depth-first manner, backtracking if no solution is found. 
            foreach (Reservation reservation in reservations) {
                List<Action> actions = Expand(state, reservation);
                actions = (from y in actions
                           select y).OrderByDescending(y => y.weight).ToList<Action>();
                // If there are no applicable actions, break and backtrack.
                if (actions.Count == 0)
                {
                    Debug.WriteLine("    Backtracking-1");
                    return null;
                }
                foreach (Action action in actions)
                {
                    State newState = state.Apply(action);
                    if (newState != null)
                    {
                        List<Reservation> remainingReservations = new List<Reservation>(reservations);
                        remainingReservations.Remove(action.reservation);
                        int newDepth = depth + 1;
                        State solution = Search(newState, remainingReservations, newDepth);
                        if (solution != null)
                        {
                            return solution;
                        }
                    }
                }
            }
             
            //closedStateList.Add(state.Repr(), "");
            Debug.WriteLine("    Backtracking-2");
            return null;
        }
        */

        public static State Search(State state, List<Reservation> reservations, int depth)
        {

            Debug.WriteLine("Reached depth: " + depth);
            if (reservations.Count == 0)
            {
                return state;
            }
            // Get applicable actions
            List<Action> actions = GetActions(state, reservations);

            actions = (from y in actions
                       select y).OrderByDescending(y => y.weight).ToList<Action>();

            // Loop for actions in a depth-first manner, backtracking if no solution is found. 
            foreach (Action action in actions)
                {
                State newState = state.Apply(action);
                if (newState != null)
                {
                    List<Reservation> remainingReservations = new List<Reservation>(reservations);
                    remainingReservations.Remove(action.reservation);
                    int newDepth = depth + 1;
                    State solution = Search(newState, remainingReservations, newDepth);
                    if(solution != null) {
                        return solution;
                    }
                }
            }
            Debug.WriteLine("    Backtracking-2");
            Debug.WriteLine(state.toString());
            return null;
        }

        public static List<Action> GetActions(State state, List<Reservation> reservations)
        {
            List<Action> actions = new List<Action>();
            foreach (Reservation reservation in reservations)
            {
                //Debug.WriteLine("Expanding reservation " + reservation.id);
                foreach (Action action in Expand(state, reservation))
                {
                    int toLane = action.leftmostLane + action.numLanes - 1;
                    //Debug.WriteLine("   For reservation " + reservation.id + "  Got an action on lane: " + action.leftmostLane + " to lane: " + toLane + " for at time: " + action.lowestTimeSlot);
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
                        Action action = new Action(lane, reservation, appWeightPair.weight);
                        actions.Add(action);
                    }
                }
            }
            return actions;
        }
    }

    class Reservation
    {
        public int id;
        public int numLanes;
        public int numTimeSlots;
        public int startTimeSlot;
        public float weight;

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
        public float weight;
        public string repr;

        public Reservation reservation;

        public Action(int leftmostLane, Reservation reservation, float weight)
        {
            this.leftmostLane = leftmostLane;
            this.lowestTimeSlot = reservation.startTimeSlot;
            this.reservation = reservation;
            this.weight = weight;
            this.numLanes = reservation.numLanes;
            this.numTimeSlots = reservation.numTimeSlots;
            this.repr = this.CalculateRepr();
        }

        public String CalculateRepr() {
            return this.numLanes + "," + this.leftmostLane + "," + this.numTimeSlots + "," + this.lowestTimeSlot;
        }
    }

    class State
    {
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
                //Debug.WriteLine("Weight for timeslot: " + reservation.startTimeSlot + "  is now:  " + this.weight[reservation.startTimeSlot]);
            }

            foreach (Reservation reservation in reservations) {
                reservation.weight = this.GetReservationWeight(reservation);
            }

            string weightString = "[";
            for (int i = 0; i < this.weight.Length; i++)
            {
                weightString += " " + this.weight[i];
            }
            weightString += "]";
            //Debug.WriteLine("Weights are now:  " + weightString);
        }


        public State Apply(Action action)
        {
            State newState = new State(this.numberOfLanes, this.numberOfTimeSlots, new List<Reservation>());
            newState.weight = this.CopyWeight(); // (float[])this.weight.Clone();
            newState.state = this.CopyState(); // (int[,])this.state.Clone();
            for (int i = action.lowestTimeSlot; i < action.lowestTimeSlot + action.numTimeSlots; i++)
            {
                for (int j = action.leftmostLane; j < action.leftmostLane + action.numLanes; j++)
                {
                    if (i < newState.numberOfTimeSlots && j < newState.numberOfLanes)
                    {
                        newState.state[i, j] = action.reservation.id;
                    }
                }
            }
            return newState;
        }

        public float[] CopyWeight() {
            float[] weight = new float[this.weight.Length];

            for (int i = 0; i < this.weight.Length; i++) { 
                weight[i] = this.weight[i];
            }
            return weight;
        }

        public int[,] CopyState()
        {
            int[,] stateCopy = new int[this.numberOfTimeSlots, this.numberOfLanes];

            for (int i = 0; i < this.numberOfTimeSlots; i++)
            {
                for (int j = 0; j < this.numberOfLanes; j++) {
                    stateCopy[i, j] = this.state[i, j];
                }
            }
            return stateCopy;
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
                    else
                    {
                        weight += this.getCellWeight(i);
                        //Debug.WriteLine("Weight updated with: " + this.getCellWeight(i) + " to: " + weight);
                    }
                }
            }
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
            float wearValue = 1.0f / wear;
            //weight += wearValue;

            // Try to spread reservations out
            // Get combined distance to other reservations
            float distance = this.getCombinedDistanceToOthers(lane, numLanes, startTimeSlot);
            // Favour those with most distance to others - but not too much.
            //Debug.WriteLine("    Weight before distance: " + weight + " distance was: " + distance);
            weight += (distance * 5.0f) / this.numberOfLanes;
            //Debug.WriteLine("    Weight ends at: " + weight);
            return new AppWeightPair(true, weight);
        }

        public float getCombinedDistanceToOthers(int lane, int numLanes, int startTimeSlot)
        {
            float distanceRight = 0.0f;
            float distanceLeft = 0.0f;
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

            float distance = Math.Min(distanceRight, distanceLeft);
            //Debug.WriteLine("        Returning distance " + distanceRight + " <min> " + distanceLeft + "  -> " + distance);
            return distance;
        }

        public float getCellWeight(int timeslot)
        {
            return this.weight[timeslot];
        }

        public float GetReservationWeight(Reservation reservation) {
            float weight = 0.0f;
            for (int i = reservation.startTimeSlot; i < reservation.startTimeSlot + reservation.numTimeSlots; i++)
            {

                for (int j = 0; j < reservation.numLanes; j++)
                {
                    weight += this.getCellWeight(i);
                    //Debug.WriteLine("Weight updated with: " + this.getCellWeight(i) + " to: " + weight);
                }
            }
            return weight;
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
                    }
                    else
                    {
                        numRepr = "" + this.state[i, j];
                    }
                    builder.Append("  " + numRepr);
                }
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public String Repr() {
            StringBuilder builder = new StringBuilder();
            for (int i = this.numberOfTimeSlots - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    builder.Append(this.state[i, j]);
                }
            }
            return builder.ToString();
        }
    }

    class AppWeightPair
    {
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
            State finalState = Scheduler.Search(thirdState, reservations, 0);
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
            State failState = Scheduler.Search(fourthState, reservations, 0);

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
            State state = Scheduler.Search(emptyState, reservations, 0);

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
            State state = Scheduler.Search(emptyState, reservations, 0);

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
            State emptyState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State state = null;
            State newState = null;
            int i = 0;
            int runs = 0;
            long timeSpent = 0;
            Random random = new Random();
            while (run)
            {
                int numLanes = 1;
                int numTimeSlots = 1;
                int startTimeSlot = 6; //  random.Next(0, numberOfTimeSlots);
                if(random.Next(0, 100) < 10) { // 10 percent will be parties and outings
                    numLanes = random.Next(2, 5);
                }else{
                    numLanes = random.Next(1, 3);
                }

                if (random.Next(0, 100) < 5) // 5 percent will be longer sessions
                { 
                    numTimeSlots = random.Next(2, 4);
                }
                else
                {
                    numTimeSlots = random.Next(1, 3);
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
                Reservation reservation = new Reservation(i+1, numLanes, numTimeSlots, startTimeSlot);
                Debug.WriteLine("Making reservation of " + numLanes + " lanes for " + numTimeSlots + " hours, at timeslot " + startTimeSlot);

                if (state != null && !state.isPossible(reservation))
                {
                    Debug.WriteLine("    It failed the first check");
                    continue;
                }

                List<Reservation> newReservations = new List<Reservation>(reservations);
                newReservations.Add(reservation);
                emptyState = new State(numberOfLanes, numberOfTimeSlots, newReservations);
                newState = Scheduler.Search(emptyState, newReservations, 0);
                long time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                timeSpent = time2 - time1;
                Debug.WriteLine("    Scheduling took: " + timeSpent + " miliseconds");
                if (newState != null)
                {
                    Debug.WriteLine("    It SUCCEEDED!!!!!!!!");
                    Debug.WriteLine(newState.toString());

                    /*
                    List<int> freecell1 = newState.getFreeCellNumbers(1);
                    string freeCellString1 = "";
                    for(int k = 0; k < freecell1.Count; k++) {
                        freeCellString1 = freeCellString1 + ", " + freecell1[k];
                    }
                    Debug.WriteLine("    Free cell for timeslot 1: " + freeCellString1);
                     */
                    reservations = newReservations;
                    state = newState;
                    i++;
                }
                else
                {
                    Debug.WriteLine("    It failed");
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
            /*if (state != null)
            {
                Debug.WriteLine(state.toString());
            }*/
            return true;
        }
    }

}