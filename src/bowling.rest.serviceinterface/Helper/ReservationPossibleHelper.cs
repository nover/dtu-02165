using Bowling.Entity.Domain;
using Bowling.Rest.Service.Model.Types;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bowling.Entity.Queries;
using bowling.scheduling;
using Microsoft.Practices.ServiceLocation;

namespace Bowling.Rest.Service.Interface.Helper
{
	class ReservationPossibleHelper
	{
		private IRepository<Reservation> reservationRepos;

		private IRepository<TimeSlot> timeSlotRepos;

		private IRepository<Lane> laneRepos;

		public ReservationPossibleHelper(IRepository<Reservation> reservation, IRepository<TimeSlot> timeslot, IRepository<Lane> lane)
		{
			this.reservationRepos = reservation;
			this.timeSlotRepos = timeslot;
			this.laneRepos = lane;
		}

		/// <summary>
		/// Determines whether the new reservation <paramref name="reservation"/> is possible
		/// based on the current reservation schedule for the day of that reservations
		/// </summary>
		/// <remarks>
		/// The lane scheduler will be queried to check for availablility. While doing this,
		/// all current reservations for that day might be rescheduled, therefore, the new schedule
		/// is returned. This schedule will have to be persisted if the new reservation is acuallly
		/// added to the database.
		/// 
		/// The out paramter "newReservation" will contain an reservation instance ready for persisting
		/// if this is sought.
		/// 
		/// Execptions will be thrown if the reservation does not fit in the current schedule
		/// </remarks>
		/// <param name="reservation">The new reservation</param>
		/// <param name="theReservation">An intance of a reservation, based on the data from <paramref name="reservation"/></param>
		/// <returns>The list of rescheduled reservations and an instance ready to commit in <paramref name="theReservation"/></returns>
		/// <exception cref="ArgumentException">If the <paramref name="reservation"/> is not possible</exception>
		public List<Reservation> Go(ReservationType reservation, out Reservation theReservation, out bool isPossible)
		{
			// quick pruning of the result - does the requested timeslot exist at all??
			TimeSlot startTimeSlot = timeSlotRepos.GetAll().FindTimeSlotStartingAt(reservation.TimeOfDay);
			if (startTimeSlot == null)
			{
				throw new ArgumentException("Reservation is not possible as the timeslot does not exist", "timeSlot");
			}

			var reservations = reservationRepos.GetAll().FindReservationsByDate(reservation.PlayAt);

			// convert these into a format the scheduler understands...
			List<LaneSchedulerReservation> schedulerReservations;
			var convertToState = ServiceLocator.Current.GetInstance<ReservationsToLaneSchedulerState>();
			var schedulerState = convertToState.Convert(reservations.ToList(), out schedulerReservations);

			LaneSchedulerReservation newReservation = new LaneSchedulerReservation()
			{
				Id = -1,
				NumberOfLanes = (int)Math.Ceiling(reservation.NumberOfPlayers / 6.0m),
				NumberOfTimeSlots = reservation.HowManyHours,
				StartTimeSlot = startTimeSlot.Id - 1
			};

			// do the scheduling
			var newState = LaneScheduler.Search(schedulerState, schedulerReservations, newReservation);

			if (newState.state == null)
			{
				throw new ArgumentException("There is no place in the schedule for this reservation", "playat");
			}

			// The scheduler attempts to place the reservation +/- one time slot in the schedule if it's full, 
			// so we have to try and handle that
			var convertToReservation = ServiceLocator.Current.GetInstance<LaneSchedulerStateToReservations>();
			var rescheduledReservations = convertToReservation.Convert(newState.state, out theReservation);
			theReservation.NumberOfPlayers = reservation.NumberOfPlayers;
			theReservation.PlayAt = reservation.PlayAt;

			// now that we have the actual reservation, we can check 
			// the added time-slots against the requested timeslot
			if (theReservation.TimeSlots[0].Start != startTimeSlot.Start)
			{
				isPossible = false;
			}
			else
			{
				isPossible = true;
			}

			return rescheduledReservations;
		}
	}
}
