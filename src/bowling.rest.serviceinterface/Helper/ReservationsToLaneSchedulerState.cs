using bowling.scheduling;
using Bowling.Entity.Domain;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Helper
{
	public class ReservationsToLaneSchedulerState
	{
		private IRepository<Lane> laneRepos;

		private IRepository<TimeSlot> timeSlotRepos;

		public ReservationsToLaneSchedulerState(IRepository<Lane> laneRepos, IRepository<TimeSlot> timeSlotRepos)
		{
			this.laneRepos = laneRepos;
			this.timeSlotRepos = timeSlotRepos;
		}
		public LaneSchedulerState Convert(IList<Reservation> reservations, out List<LaneSchedulerReservation> schedulerReservations)
		{			
			schedulerReservations = (from y in reservations
										 select new LaneSchedulerReservation()
										 {
											 Id = y.Id,
											 NumberOfLanes = (int)Math.Ceiling(y.NumberOfPlayers / 6.0m),
											 NumberOfTimeSlots = y.TimeSlots.Count,
											 StartTimeSlot = y.TimeSlots[0].Id
										 }).ToList();
			
			var state =  new LaneSchedulerState(
				this.laneRepos.GetAll().Count(),
				this.timeSlotRepos.GetAll().Count(),
				schedulerReservations);
			// iterate all of the reservations in order to put them into the internal state array of the sched.
			int[,] internalState = state.State;
			int laneId = 0;
			int slotId = 0;
			foreach (var resv in reservations)
			{
				foreach (var lane in resv.Lanes)
				{
					foreach (var slot in resv.TimeSlots)
					{
						laneId = lane.Id-1;
						slotId = slot.Id-1;
						if (internalState[slotId, laneId] != 0)
						{
							throw new InvalidOperationException(String.Format("The scheduler state at position  {0}, {1} is already taken. This means that one or more reservations have been allocated to the same lane and slot. Not so good"));
						}
						internalState[slotId, laneId] = resv.Id;
					}
				}
			}
			state.State = internalState;

			return state;
			
		}
	}
}
