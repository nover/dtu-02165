/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
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
}
*/
