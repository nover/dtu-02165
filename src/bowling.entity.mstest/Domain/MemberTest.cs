using bowling.entity.mstest;
using Bowling.Entity.Domain;
using FluentNHibernate;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateSrc.Tests.Domain
{
    [TestClass]
    public class MemberTest : EntityTestBase
    {
        [TestMethod]
        public void TestMemberCRUD()
        {
            new PersistenceSpecification<Bowling.Entity.Domain.Member>(this._session)
                .CheckProperty(c => c.Id, 1)
                .CheckProperty(c => c.Name, "John Doe")
                .CheckProperty(c => c.Email, "john@doe.dk")
                .CheckProperty(c => c.Password, "blabla")
                .CheckProperty(c => c.Title, MemberTitle.Mr)
                .CheckProperty(c => c.DialCode, "+45")
                .CheckProperty(c => c.CellPhone, 28123456)
                .CheckProperty(c => c.DefaultNumberOfPlayers, 2)
                .CheckProperty(c => c.ReceiveNewsLetter, true)
                .VerifyTheMappings();
        }
    }
}
