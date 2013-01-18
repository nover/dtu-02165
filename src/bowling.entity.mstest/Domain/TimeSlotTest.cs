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
    public class TimeSlotTest : EntityTestBase
    {
        [TestMethod]
        public void TestTimeSlotCRUD()
        {
            new PersistenceSpecification<Bowling.Entity.Domain.TimeSlot>(this._session)
                 .CheckProperty(c => c.Id, 1)
                 .CheckProperty(c => c.Start, new TimeSpan(12, 0, 0))
                 .CheckProperty(c => c.End, new TimeSpan(13, 0, 0))
                 .VerifyTheMappings();
        }
    }
}
