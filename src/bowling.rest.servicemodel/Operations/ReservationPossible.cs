using Bowling.Rest.Service.Model.Types;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
	[Route("/reservation/possible")]
	public class ReservationPossible
	{
		public ReservationPossible()
		{
			this.Reservation = new ReservationType();
		}
		public ReservationType Reservation { get; set; }
	}
}
