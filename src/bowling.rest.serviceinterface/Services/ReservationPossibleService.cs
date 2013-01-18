using Bowling.Entity.Domain;
using Bowling.Rest.Service.Model.Operations;
using Microsoft.Practices.ServiceLocation;
using ServiceStack.ServiceInterface;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bowling.Entity.Queries;
using bowling.scheduling;

namespace Bowling.Rest.Service.Interface.Services
{
	public class ReservationPossibleService : RestServiceBase<ReservationPossible>
	{
		public override object OnGet(ReservationPossible request)
		{
			var response = new ReservationPossibleResponse();

			// get all of the reservations of the day that the request embeds
			var reservationRepos = ServiceLocator.Current.GetInstance<IRepository<Reservation>>();
			var timeSlotRepos = ServiceLocator.Current.GetInstance<IRepository<TimeSlot>>();
			var laneRepos = ServiceLocator.Current.GetInstance<IRepository<Lane>>();

			// quick pruning of the result - does the requested timeslot exist at all??
			TimeSlot startTimeSlot = timeSlotRepos.GetAll().FindTimeSlotStartingAt(request.Reservation.TimeOfDay);
			if (startTimeSlot == null)
			{
				return response;
			}

			var reservations = reservationRepos.GetAll().FindReservationsByDate(request.Reservation.PlayAt);

			// convert these into a format the scheduler understands...
			var schedulerReservations = (from y in reservations
										 select new LaneSchedulerReservation()
										 {
											 Id = y.Id,
											 NumberOfLanes = (int)Math.Ceiling(y.NumberOfPlayers / 6.0m),
											 NumberOfTimeSlots = y.TimeSlots.Count,
											 StartTimeSlot = y.TimeSlots.First().Id
										 }).ToList();
			// construct state
			LaneSchedulerState schedulerState = new LaneSchedulerState(
				laneRepos.GetAll().Count(), 
				timeSlotRepos.GetAll().Count(),
				schedulerReservations);

			LaneSchedulerReservation newReservation = new LaneSchedulerReservation() {
				Id = -1,
				NumberOfLanes = (int)Math.Ceiling(request.Reservation.NumberOfPlayers / 6.0m),
				NumberOfTimeSlots = request.Reservation.HowManyHours,
				StartTimeSlot = startTimeSlot.Id
			};

			// do the scheduling
			var newState = LaneScheduler.Search(schedulerState, schedulerReservations, newReservation);

			if (newState == null)
			{
				return response;
			}

			response.IsPossible = true;

			return response;
		}
	}
}
