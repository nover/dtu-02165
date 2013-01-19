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
		public override object OnPost(ReservationPossible request)
		{
			var response = new ReservationPossibleResponse();

			Reservation theReservation;
			bool isPossible;
			var helper = ServiceLocator.Current.GetInstance<ReservationPossibleHelper>();

			try
			{
				helper.Go(request.Reservation, out theReservation, out isPossible);

				response.IsPossible = isPossible;
				return response;
			}
			catch (ArgumentException ex)
			{
				response.IsPossible = false;
				//TODO add suggestions here!

				return response;
			}
		}
	}
}
