using Bowling.Rest.Service.Model.Types;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
	[Route("/reservation")]
	public class Reservations
	{
		#region GET on the collection
		/// <summary>
		/// Used for GET on the collection to fetch all the reservations on a given date.
		/// </summary>
		public DateTime? Date { get; set; }
		#endregion

		#region POST to the service
		/// <summary>
		/// Used for POST operation to create a new reservation
		/// </summary>
		public ReservationType Reservation { get; set; }
		#endregion
		
		public Reservations()
		{
			this.Reservation = new ReservationType();
		}
	}
	
}
