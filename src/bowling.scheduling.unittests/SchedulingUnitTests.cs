using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bowling.scheduling;
using System.Collections.Generic;
using System.Diagnostics;

namespace bowling.scheduling.unittests
{
    [TestClass]
    public class SchedulingUnitTests
    {
        [TestMethod]
        public void TestMethod1_SmallReservation()
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
            State finalState = Scheduler.RecursiveSearch(thirdState, reservations, 0, 100, 0);
       
            Assert.IsNotNull(finalState, "Final state was null, which is unexpected");

            Debug.WriteLine(finalState.toString());
        }
    }
}
