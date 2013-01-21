using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bowling.Entity.Domain;
using Bowling.Entity.Queries;
using System.Linq;
using Bowling.Rest.Service.Interface.Services;
using Bowling.Rest.Service.Model.Operations;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;
namespace bowling.rest.tests.Services
{
    [TestClass]
    public class ReservationServiceTest : ServiceTestBase
    {
        [TestMethod]
        public void TestGetReservationsForGivenDay()
        {
            for (int i = 0; i < this.lanes.Count-5; i++)
            {
                var reservation = new Reservation()
                {
                    CreatedAt = DateTime.Now,
                    Name = "Gert",
                    NumberOfPlayers = 4,
                    PhoneNumber = 1234,
                    PlayAt = DateTime.Now
                };

                reservation.AddLane(this.lanes[i]);
                reservation.AddTimeSlot((from y in this.timeSlots select y).First<TimeSlot>());

                this._session.Save(reservation);
            }

            var reservationService = new ReservationsService();
            var request = new Reservations {
                Date = DateTime.Now
            };

            ReservationsResponse response = reservationService.OnGet(request) as ReservationsResponse;
            Assert.That(response.ReservationList.Count, Is.EqualTo(5), "We expect that there are 5 reservations today");

            foreach (var r in response.ReservationList)
            {
                Assert.That(r, Is.Not.Null);
            }

        }
    }
}
