using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bowling.Entity.Domain;
using FluentNHibernate.Testing;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;

namespace bowling.entity.mstest.Domain
{
	[TestClass]
	public class ReservationTest : EntityTestBase
	{
		[TestMethod]
		public void TestReservationCRUD()
		{
			var theLanes = new Lane[] { 
						new Lane { 
							Name = "1" }, 
						new Lane { 
							Name = "2" }, 
						new Lane { 
							Name = "3" } 
						};
			var theSlots = new TimeSlot[] { 
						new TimeSlot { 
							Start = new TimeSpan(13, 0, 0),
							End = new TimeSpan(14, 0,0) }, 
						new TimeSlot { 
							Start = new TimeSpan(14, 0, 0),
							End = new TimeSpan(15, 0,0) }, 
						new TimeSlot { 
							Start = new TimeSpan(15, 0, 0),
							End = new TimeSpan(16, 0,0)} 
						};

			foreach (var lane in theLanes)
			{
				this._session.Save(lane);
			}
			foreach (var slot in theSlots)
			{
				this._session.Save(slot);
			}


			new PersistenceSpecification<Reservation>(this._session)
			  .CheckProperty(c => c.Id, 1)
			  .CheckProperty(c => c.Name, "John Doe")
			  .CheckProperty(c => c.CreatedAt, DateTime.Now, new DateTimeEqualityComparer())
			  .CheckProperty(c => c.NumberOfPlayers, 6)
			  .CheckProperty(c => c.PhoneNumber, "12345678")
			  .CheckProperty(c => c.PlayAt, DateTime.Now.AddDays(1), new DateTimeEqualityComparer())
			  .CheckProperty(c => c.Status, ReservationStatus.Pending)
			  .CheckReference(
					c => c.Member,
					new Member()
					{
						Email = "my@email.dk",
						DefaultNumOfPlayers = 42,
						DialCode = "+45",
						Name = "John Doe",
						Password = "1234",
						ReceiveNewsLetter = true,
						Title = "Mr"
					})
			   .CheckComponentList(
					x => x.Lanes, 
					theLanes,(r, l) => {
							r.AddLane(l);
						})
			  .CheckComponentList(
					x => x.TimeSlots,
				   theSlots, (r, t) => {
							r.AddTimeSlot(t);
						})
			  .VerifyTheMappings();
		}

		[TestMethod]
		public void TestAddingSameLaneReturnsFalse()
		{
			var lane1 = new Lane { Name = "42" };
			var lane2 = new Lane { Name = "45" };
			this._session.Save(lane1);
			this._session.Save(lane2);

			Reservation resv = new Reservation();

			Assert.That(resv.AddLane(lane1), Is.True);
			Assert.That(resv.AddLane(lane2), Is.True);
			Assert.That(resv.AddLane(lane1), Is.False);
		}

		[TestMethod]
		public void TestAddingSameTimeSlotReturnsFalse()
		{
			var slot1 = new TimeSlot { Start = TimeSpan.FromHours(10), End = TimeSpan.FromHours(11) };
			var slot2 = new TimeSlot { Start = TimeSpan.FromHours(11), End = TimeSpan.FromHours(12) };
			this._session.Save(slot1);
			this._session.Save(slot2);

			Reservation resv = new Reservation();
			Assert.That(resv.AddTimeSlot(slot1), Is.True);
			Assert.That(resv.AddTimeSlot(slot2), Is.True);
			Assert.That(resv.AddTimeSlot(slot2), Is.False);
		}
	}
}
