using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bowling.Rest.Service.Interface.Services;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Model.Types;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;

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
					TimeOfDay = TimeSpan.FromHours(13)
				}
			};
			
			ReservationPossibleResponse response = service.OnGet(request) as ReservationPossibleResponse;
			Assert.That(response.IsPossible, Is.True);
		}
	}
}
