using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.entity.mstest
{
    [TestClass]
    public class LaneTest : EntityTestBase
    {
        [TestMethod]
        public void TestLaneCRUD()
        {
            new PersistenceSpecification<Bowling.Entity.Domain.Lane>(this._session)
            .CheckProperty(c => c.Name, 2)
            .VerifyTheMappings();
        }
    }
}
