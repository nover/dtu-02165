using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bowling.Rest.Service.Model.Types;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Interface.Validation;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;
namespace bowling.rest.tests.Validator
{
	[TestClass]
	public class ReservationPossibleValidatorTest
	{
		[TestMethod]
		public void TestReservationPossibleValidRequest1()
		{
			ReservationType resv = new ReservationType()
			{
				HowManyHours = 2,
				NumberOfPlayers = 20,
				PlayAt = DateTime.Now.AddDays(1),
				TimeOfDay = TimeSpan.FromHours(13)
			};

			ReservationPossible request = new ReservationPossible() { Reservation = resv };
			ReservationPossibleValidator validator = new ReservationPossibleValidator();
			Assert.That(validator.Validate(request).IsValid, Is.True);
		}

		[TestMethod]
		public void TestReservationPossibleInvalidHowManyHours()
		{
			ReservationType resv = new ReservationType()
			{
				HowManyHours = 42,
				NumberOfPlayers = 20,
				PlayAt = DateTime.Now.AddDays(1),
				TimeOfDay = TimeSpan.FromHours(13)
			};

			ReservationPossible request = new ReservationPossible() { Reservation = resv };
			ReservationPossibleValidator validator = new ReservationPossibleValidator();
			var response = validator.Validate(request);
			Assert.That(response.IsValid, Is.False);
			Assert.That(response.Errors[0].PropertyName, Is.EqualTo("Reservation.HowManyHours"));
		}

		[TestMethod]
		public void TestReservationPossibleInvalidNumberOfPlayers()
		{
			ReservationType resv = new ReservationType()
			{
				HowManyHours = 3,
				NumberOfPlayers = 66,
				PlayAt = DateTime.Now.AddDays(1),
				TimeOfDay = TimeSpan.FromHours(13)
			};

			ReservationPossible request = new ReservationPossible() { Reservation = resv };
			ReservationPossibleValidator validator = new ReservationPossibleValidator();
			var response = validator.Validate(request);
			Assert.That(response.IsValid, Is.False);
			Assert.That(response.Errors[0].PropertyName, Is.EqualTo("Reservation.NumberOfPlayers"));
		}

		[TestMethod]
		public void TestReservationPossibleInvalidPlayAt()
		{
			ReservationType resv = new ReservationType()
			{
				HowManyHours = 3,
				NumberOfPlayers = 20,
				PlayAt = DateTime.Now.AddDays(-1),
				TimeOfDay = TimeSpan.FromHours(13)
			};

			ReservationPossible request = new ReservationPossible() { Reservation = resv };
			ReservationPossibleValidator validator = new ReservationPossibleValidator();
			var response = validator.Validate(request);
			Assert.That(response.IsValid, Is.False);
			Assert.That(response.Errors[0].PropertyName, Is.EqualTo("Reservation.PlayAt"));
		}

		[TestMethod]
		public void TestReservationPossibleMultipleInvalid()
		{
			ReservationType resv = new ReservationType()
			{
				HowManyHours = -1,
				NumberOfPlayers = 44,
				PlayAt = DateTime.Now.AddDays(1),
				TimeOfDay = TimeSpan.FromHours(13)
			};

			ReservationPossible request = new ReservationPossible() { Reservation = resv };
			ReservationPossibleValidator validator = new ReservationPossibleValidator();
			var response = validator.Validate(request);
			Assert.That(response.IsValid, Is.False);
			Assert.That(response.Errors[0].PropertyName, Is.EqualTo("Reservation.HowManyHours"));
			Assert.That(response.Errors[1].PropertyName, Is.EqualTo("Reservation.NumberOfPlayers"));
		}
	}
}

