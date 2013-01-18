using Bowling.Rest.Service.Model.Operations;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Validation
{
	public class ReservationPossibleValidator : AbstractValidator<ReservationPossible>
	{
		public ReservationPossibleValidator()
		{
			RuleFor(x => x.Reservation.HowManyHours).ExclusiveBetween(1, 4);
			RuleFor(x => x.Reservation.NumberOfPlayers).InclusiveBetween(1, 24);
			RuleFor(x => x.Reservation.PlayAt).GreaterThanOrEqualTo(DateTime.Now);
			RuleFor(x => x.Reservation.TimeOfDay.Hours).GreaterThanOrEqualTo(0);
		}
	}
}
