using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bowling.Rest.Service.Interface.Services;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Model.Types;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;
using Bowling.Entity.Domain;
using System.Linq;

namespace bowling.rest.tests.Services
{
	[TestClass]
	public class ReservationPossibleServiceTest : ServiceTestBase
	{
		[TestMethod]
		public void TestValidReservationWithNoOtherReservations()
		{
			var service = new ReservationPossibleService();


			var request = new ReservationPossible() {
				Reservation = new ReservationType() {
					HowManyHours = 2,
					NumberOfPlayers = 4,
					PlayAt = DateTime.Now,
					TimeOfDay = TimeSpan.FromHours(10)
				}
			};
			
			ReservationPossibleResponse response = service.OnGet(request) as ReservationPossibleResponse;
			Assert.That(response.IsPossible, Is.True);
		}

		[TestMethod]
		public void TestValidResrvationWithSomeOtherReservations()
		{
			var reservation = new Reservation()
			{
				CreatedAt = DateTime.Now,
				Name = "Gert",
				NumberOfPlayers = 4,
				PhoneNumber = "1234",
				PlayAt =DateTime.Now
			};

			reservation.AddLane((from y in this.lanes select y).First<Lane>());
			reservation.AddTimeSlot((from y in this.timeSlots select y).First<TimeSlot>());

			this._session.Save(reservation);

			var service = new ReservationPossibleService();

			var request = new ReservationPossible()
			{
				Reservation = new ReservationType()
				{
					HowManyHours = 2,
					NumberOfPlayers = 8,
					PlayAt = DateTime.Now,
					TimeOfDay = (from y in this.timeSlots select y).First<TimeSlot>().Start
				}
			};

			ReservationPossibleResponse response = service.OnGet(request) as ReservationPossibleResponse;
			Assert.That(response.IsPossible, Is.True);
		}
		[TestMethod]
		public void TestImpossibleReservationsDueToLackOfTimeSlots()
		{
			for (int i = 0; i < this.lanes.Count; i++)
			{
				var reservation = new Reservation()
				{
					CreatedAt = DateTime.Now,
					Name = "Gert",
					NumberOfPlayers = 4,
					PhoneNumber = "1234",
					PlayAt = DateTime.Now
				};

				reservation.AddLane(this.lanes[i]);
				reservation.AddTimeSlot((from y in this.timeSlots select y).First<TimeSlot>());

				this._session.Save(reservation);
			}
			// since we filled out all lanes at that timeslot, the next booking for that timeslot should not be possible
			var service = new ReservationPossibleService();
			var request = new ReservationPossible()
			{
				Reservation = new ReservationType()
				{
					HowManyHours = 2,
					NumberOfPlayers = 8,
					PlayAt = DateTime.Now,
					TimeOfDay = (from y in this.timeSlots select y).First<TimeSlot>().Start
				}
			};

			ReservationPossibleResponse response = service.OnGet(request) as ReservationPossibleResponse;
			Assert.That(response.IsPossible, Is.False);
			Assert.That(response.Suggestions.Count, Is.EqualTo(1));
			Assert.That(response.Suggestions[0].HowManyHours, Is.EqualTo(request.Reservation.HowManyHours));
			Assert.That(response.Suggestions[0].NumberOfPlayers, Is.EqualTo(request.Reservation.NumberOfPlayers));
			Assert.That(response.Suggestions[0].PlayAt, Is.EqualTo(request.Reservation.PlayAt));
			Assert.That(response.Suggestions[0].TimeOfDay, Is.Not.EqualTo(request.Reservation.TimeOfDay));
		}
	}
}
