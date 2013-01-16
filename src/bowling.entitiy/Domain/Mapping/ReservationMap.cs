using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain.Mapping
{
    class ReservationMap : ClassMap<Reservation>
    {
        public ReservationMap()
        {
            Map(x => x.PlayAt);
            Map(x => x.NumOfPlayers);
            Map(x => x.Name);
            Map(x => x.PhoneNumber);
            References<Member>(x => x.Member);
            Map(x => x.Status).CustomSqlType("string");
            Map(x => x.CreatedAt);
        }
    }
}
