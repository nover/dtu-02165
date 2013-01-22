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
using Bowling.Rest.Service.Interface.Helper;
using AutoMapper;
using Bowling.Rest.Service.Model.Types;

namespace Bowling.Rest.Service.Interface.Services
{
	public class ReservationPossibleService : RestServiceBase<ReservationPossible>
	{
        private IRepository<TimeSlot> timeSlotRepos;

		public override object OnPost(ReservationPossible request)
		{
			var response = new ReservationPossibleResponse();

			Reservation theReservation;
			bool isPossible;
            var suggestions = new List<LaneSchedulerReservation>();
			var helper = ServiceLocator.Current.GetInstance<ReservationPossibleHelper>();

			helper.Go(request.Reservation, out theReservation, out isPossible, out suggestions);
            response.IsPossible = isPossible;
            if(!isPossible)
            {
                // TODO do suggestion dance and return
               // response.Suggestions=Mapper.Map<List<ReservationType>>(suggestions);

                var newReservation = new List<Reservation>();
                var timeslotrepos = ServiceLocator.Current.GetInstance<IRepository<TimeSlot>>();

                foreach (var suggestion in suggestions)
                {
                    // the scheduler is using 0 based indexes for it's search space
                    // so we add one to get the actual DB id.
                    var startTimeSlot = timeslotrepos.Get(suggestion.StartTimeSlot + 1);
                    response.Suggestions.Add(new ReservationType
                    {
                        HowManyHours = suggestion.NumberOfTimeSlots,
                        NumberOfPlayers = request.Reservation.NumberOfPlayers,
                        PlayAt = request.Reservation.PlayAt,
                        TimeOfDay = startTimeSlot.Start
                    });
                }
            }
            
            return response;
		}
	}
}
