using Bowling.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Queries
{
	public static class TimeSlotQueryExtension
	{
		/// <summary>
		/// Finds a single time slot which starts at the given time of day
		/// </summary>
		/// <remarks>
		/// The assumption is that time slots are on an hourly basis, so only the hour 
		/// part of the TimeSpan is examined.
		/// 
		/// If the timeslot does not exist, null is returned.
		/// </remarks>
		/// <param name="slots">IQueryable over all of the time slots in the system</param>
		/// <param name="timeOfDay">The time of day the Start property of TimeSlot should have</param>
		/// <returns></returns>
		public static TimeSlot FindTimeSlotStartingAt(this IQueryable<TimeSlot> slots, TimeSpan timeOfDay)
		{
			return (from y in slots
					where y.Start.Hours == timeOfDay.Hours
					select y).SingleOrDefault();
		}
	}
}
