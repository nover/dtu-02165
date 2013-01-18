﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    class PerformanceTests
    {
        public static bool Test_n_reservations(int numberOfLanes, int numberOfTimeSlots, int numberOfVisitors, int runLimit)
        {
            Debug.WriteLine("Testing scheduling of " + numberOfVisitors + " visitors in " + numberOfLanes + " lanes and " + numberOfTimeSlots + " timeslots");
            WearData.Populate(numberOfLanes);
            List<Reservation> reservations = new List<Reservation>();
            // Reservation(int id, int numLanes, int numTimeSlots, int startTimeSlot)
            bool run = true;
            //State emptyState = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State state = new State(numberOfLanes, numberOfTimeSlots, reservations);
            State newState = null;
            int i = 0;
            int visitors = 0;
            int runs = 0;
            long timeSpent = 0;
            Random random = new Random();
            while (run)
            {
                int numVisitors = 4;

                int numTimeSlots = random.Next(1, 3);
                int startTimeSlot = 6; //  random.Next(0, numberOfTimeSlots);
                if (random.Next(0, 100) < 10)
                { // 10 percent will be parties and outings
                    numVisitors = random.Next(5, 41);
                }
                else
                {
                    numVisitors = random.Next(3, 7);
                }

                if (random.Next(0, 100) < 15)
                {
                    startTimeSlot = random.Next(0, numberOfTimeSlots);
                }
                else
                {
                    startTimeSlot = random.Next(2, numberOfTimeSlots - 2);
                }
                int numLanes = 0;
                if (numVisitors % 6 == 0)
                {
                    numLanes = numVisitors / 6;
                }
                else
                {
                    numLanes = (numVisitors / 6) + 1;
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
                    Debug.WriteLine(newState.ToString());

                    reservations = newReservations;
                    state = newState;
                    visitors = visitors + numVisitors;
                    Debug.WriteLine("Have now scheduled for: " + visitors + " visitors");
                    i++;
                }
                else
                {
                    Debug.WriteLine("    It failed. Alternative reservations are:");
                    for (int j = 0; j < result.reservations.Count; j++)
                    {
                        Debug.WriteLine("        Reservation for " + result.reservations[j].numLanes + " lanes at slot: " + result.reservations[j].startTimeSlot + " for " + result.reservations[j].numTimeSlots + " hours");
                    }
                }
                if (visitors > numberOfVisitors)
                {
                    run = false;
                }
                if (runs > runLimit)
                {
                    run = false;
                }
                runs++;
            }
            Debug.WriteLine("The last scheduling took: " + timeSpent + " miliseconds");
            if (state != null)
            {
                Debug.WriteLine(state.ToString());
            }
            return true;
        }
    }
}
