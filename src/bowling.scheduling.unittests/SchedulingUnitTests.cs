using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            LaneWearData.Populate(numberOfLanes);
            List<LaneSchedulerReservation> reservations = new List<LaneSchedulerReservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            LaneSchedulerReservation res1 = new LaneSchedulerReservation(1, 2, 2, 3);
            LaneSchedulerReservation res2 = new LaneSchedulerReservation(2, 2, 2, 2);
            LaneSchedulerReservation res3 = new LaneSchedulerReservation(3, 1, 2, 2);

            // add reservations
            reservations.Add(res1);
            reservations.Add(res2);
            reservations.Add(res3);

            // Test Search
            LaneSchedulerState thirdState = new LaneSchedulerState(numberOfLanes, numberOfTimeSlots, reservations);
            LaneSchedulerState finalState = LaneScheduler.RecursiveSearch(thirdState, reservations, 0, 100, 0);
       
            Assert.IsNotNull(finalState, "Final state was null, which is unexpected");
        }

        [TestMethod]
        public void TestMethod2_FailingReservation()
        {
            int numberOfLanes = 6;
            int numberOfTimeSlots = 5;
            LaneWearData.Populate(numberOfLanes);
            List<LaneSchedulerReservation> reservations = new List<LaneSchedulerReservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            LaneSchedulerReservation res1 = new LaneSchedulerReservation(1, 2, 2, 3);
            LaneSchedulerReservation res2 = new LaneSchedulerReservation(2, 2, 2, 2);
            LaneSchedulerReservation res3 = new LaneSchedulerReservation(3, 1, 2, 2);
            LaneSchedulerReservation res4 = new LaneSchedulerReservation(4, 6, 2, 3);

            // Test Search
            reservations.Add(res1);
            reservations.Add(res2);
            reservations.Add(res3);
            reservations.Add(res4);
            LaneSchedulerState fourthState = new LaneSchedulerState(numberOfLanes, numberOfTimeSlots, reservations);
            LaneSchedulerState failState = LaneScheduler.RecursiveSearch(fourthState, reservations, 0, 0, 0);

            Assert.IsNull(failState, "Unschedulable state appears to have been scheduled!");
        }

        [TestMethod]
        public void TestMethod3_OptimalPlacement()
        {
            int numberOfLanes = 6;
            int numberOfTimeSlots = 6;
            LaneWearData.Populate(numberOfLanes);
            List<LaneSchedulerReservation> reservations = new List<LaneSchedulerReservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            LaneSchedulerReservation res1 = new LaneSchedulerReservation(1, 3, 2, 2);
            LaneSchedulerReservation res2 = new LaneSchedulerReservation(2, 3, 2, 2);

            // Test Search
            reservations.Add(res1);
            reservations.Add(res2);

            LaneSchedulerState emptyState = new LaneSchedulerState(numberOfLanes, numberOfTimeSlots, reservations);
            LaneSchedulerState state = LaneScheduler.RecursiveSearch(emptyState, reservations, 0, 1000, 0);

            // TODO: Should we check for other things than schedulability in this test?
            Assert.IsNotNull(state, "Final state was null, which is unexpected");
        }
        
        [TestMethod]
        public void TestMethod4_ProblematicReservation()
        {
            int numberOfLanes = 6;
            int numberOfTimeSlots = 8;
            LaneWearData.Populate(numberOfLanes);
            List<LaneSchedulerReservation> reservations = new List<LaneSchedulerReservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            LaneSchedulerReservation res1 = new LaneSchedulerReservation(1, 3, 2, 0);
            LaneSchedulerReservation res2 = new LaneSchedulerReservation(2, 3, 2, 1);
            LaneSchedulerReservation res3 = new LaneSchedulerReservation(3, 1, 4, 2);
            LaneSchedulerReservation res4 = new LaneSchedulerReservation(4, 5, 2, 5);
            LaneSchedulerReservation res5 = new LaneSchedulerReservation(5, 2, 2, 2);

            // Test Search
            reservations.Add(res1);
            reservations.Add(res2);
            reservations.Add(res3);
            reservations.Add(res4);
            reservations.Add(res5);

            LaneSchedulerState emptyState = new LaneSchedulerState(numberOfLanes, numberOfTimeSlots, reservations);
            LaneSchedulerState state = LaneScheduler.RecursiveSearch(emptyState, reservations, 0, 1000, 0);

            // TODO: Should we check for other things than schedulability in this test?
            Assert.IsNotNull(state, "Final state was null, which is unexpected");
        }
    }
}
