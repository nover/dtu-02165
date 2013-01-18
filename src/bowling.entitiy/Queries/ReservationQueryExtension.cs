using Bowling.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Queries
{
	public static class ReservationQueryExtension
	{
		/// <summary>
		/// Finds all reservations for a given date
		/// </summary>
		/// <param name="query"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public static IQueryable<Reservation> FindReservationsByDate(this IQueryable<Reservation> query, DateTime date)
		{
			return from y in query
				   where y.PlayAt.Date == date.Date
				   select y;
		}
	}
}
