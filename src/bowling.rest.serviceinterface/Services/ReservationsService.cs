﻿using AutoMapper;
using Bowling.Entity.Domain;
using Bowling.Rest.Service.Interface.Helper;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Model.Types;
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
	public class ReservationsService : RestServiceBase<Reservations>
	{
		public override object OnGet(Reservations request)
		{
			var reservationRepos = ServiceLocator.Current.GetInstance<IRepository<Reservation>>();
			var reservations = reservationRepos.GetAll().FindReservationsByDate(request.Date.Value);

			var response = new ReservationsResponse();

			foreach (var r in reservations)
			{
				response.ReservationList.Add(Mapper.Map<ReservationType>(r));
			}

			return response;
		}
		public override object OnPost(Reservations request)
		{
			// first check if the reservation is possible, then save the full reschedule including
			// the new reservation

			Reservation reservation;
			bool isPossible;
            var suggestions = new List<LaneSchedulerReservation>();
			var helper = ServiceLocator.Current.GetInstance<ReservationPossibleHelper>();
			var reschedule = helper.Go(request.Reservation, out reservation, out isPossible, out suggestions);
			if (!isPossible)
			{
				throw new InvalidOperationException("Your requested reservation is not possible, sorry");
			}
			if (request.Reservation.MemberId != null)
			{
				var memberRepos = ServiceLocator.Current.GetInstance<IRepository<Member>>();
				reservation.Member = memberRepos.Get(request.Reservation.MemberId.Value);
			}

			var repos = ServiceLocator.Current.GetInstance<IRepository<Reservation>>();
			reschedule.Add(reservation);
			repos.DbContext.BeginTransaction();

			foreach (var r in reschedule)
			{
				repos.SaveOrUpdate(r);
			}

			repos.DbContext.CommitTransaction();

			return new ReservationsResponse()
			{
				Reservation = Mapper.Map<ReservationType>(reservation)
			};
		}

	}
}
