using bowling.scheduling;
using Bowling.Entity.Domain;
using Bowling.Rest.Service.Interface.Helper.Internal;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Helper
{
	public class LaneSchedulerStateToReservations
	{
		private IRepository<Lane> laneRepos;

		private IRepository<TimeSlot> timeSlotRepos;

		private IRepository<Reservation> reservationRepos;

		public LaneSchedulerStateToReservations(IRepository<Lane> laneRepos, IRepository<TimeSlot> timeSlotRepos, IRepository<Reservation> reservationRepos)
		{
			this.laneRepos = laneRepos;
			this.timeSlotRepos = timeSlotRepos;
			this.reservationRepos = reservationRepos;
		}

		public List<Reservation> Convert(LaneSchedulerState state, out Reservation newReservation)
		{
			int [,] internalState = state.State;
			int current = 0;
			newReservation = new Reservation();
			int laneCount = this.laneRepos.GetAll().Count();
			int timeSlotCount = this.timeSlotRepos.GetAll().Count();

			List<ReservationLaneTimeSlotPair> pairs = new List<ReservationLaneTimeSlotPair>();

			for (int i = 0; i < laneCount; i++)
			{
				for (int j = 0; j < timeSlotCount; j++)
				{
					current = internalState[i, j];
					if (current == 0)
					{
						continue;
					}
					var pair = new ReservationLaneTimeSlotPair()
					{
						LaneId = i+1,
						TimeSlotId = j+1,
						ReservationId = current
					};

					pairs.Add(pair);
				}
			}

			// load all reservations besides -1 by first gettting the distinct list of
			// ids, then loading these from db and converting to a dictionary
			// then finally clearing all lane and timeslots from these
			var distinctReservations = (from y in pairs
											where y.ReservationId != -1
											select y.ReservationId).Distinct<int>().ToList();
			var distinctTimeSlots = (from y in pairs
									 select y.TimeSlotId).Distinct<int>().ToList();
			var distinctLanes = (from y in pairs
								 select y.LaneId).Distinct<int>().ToList();

			var reservations = (from y in reservationRepos.GetAll()
								where distinctReservations.Contains(y.Id)
								select y).ToDictionary(t => t.Id, t => t);

			var timeslots = (from y in timeSlotRepos.GetAll()
							 where distinctTimeSlots.Contains(y.Id)
							 select y).ToDictionary(t => t.Id, t => t);
			
			var lanes = (from y in laneRepos.GetAll()
						 where distinctLanes.Contains(y.Id)
						 select y).ToDictionary(t => t.Id, t => t);

			foreach (var r in reservations)
			{
				r.Value.Lanes.Clear();
				r.Value.TimeSlots.Clear();
			}

			// now we can reschedule all of the reservations, and add the lanes and timeslots 
			// to the new reservation as well
			foreach (var pair in pairs)
			{
				if (pair.ReservationId == -1)
				{
					newReservation.AddLane(lanes[pair.LaneId]);
					newReservation.AddTimeSlot(timeslots[pair.TimeSlotId]);
					continue;
				}

				reservations[pair.ReservationId].AddLane(lanes[pair.LaneId]);
				reservations[pair.ReservationId].AddTimeSlot(timeslots[pair.TimeSlotId]);

			}

			var toReturn = new List<Reservation>();
			foreach(var r in reservations)
			{
				toReturn.Add(r.Value);
			}
			distinctLanes = null;
			distinctReservations = null;
			distinctTimeSlots = null;

			return toReturn;
		}
	}
}
