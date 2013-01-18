using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bowling.Entity.Domain;
using FluentNHibernate.Testing;

namespace bowling.entity.mstest.Domain
{
    [TestClass]
    public class ReservationTest : EntityTestBase
    {
        [TestMethod]
        public void TestReservationCRUD()
        {
            new PersistenceSpecification<Reservation>(this._session)
              .CheckProperty(c => c.Id, 1)
              .CheckProperty(c => c.Name, "John Doe")
              .CheckProperty(c=>c.CreatedAt, DateTime.Now, new DateTimeEqualityComparer())
              .CheckProperty(c => c.NumberOfPlayers, 6)
              .CheckProperty(c => c.PhoneNumber, "12345678")
              .CheckProperty(c => c.PlayAt, DateTime.Now.AddDays(1), new DateTimeEqualityComparer())
              .CheckProperty(c => c.Status, ReservationStatus.Pending)
              .CheckReference(
                    c=>c.Member, 
                    new Member() {
                        Email = "my@email.dk", 
                        DefaultNumOfPlayers=42, 
                        DialCode = "+45",
                        Name = "John Doe",
                        Password = "1234", 
                        ReceiveNewsLetter = true,
                        Title = "Mr"
                    })
              .VerifyTheMappings();
        }
    }
}
