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
            Id(x => x.Id);
            Map(x => x.PlayAt);
            Map(x => x.NumberOfPlayers);
            Map(x => x.Name);
            Map(x => x.PhoneNumber);
            References<Member>(x => x.Member).Not.LazyLoad();
            Map(x => x.Status).CustomSqlType("string");
            Map(x => x.CreatedAt);
            HasManyToMany(x => x.Lanes).Cascade.None().Not.LazyLoad();
            HasManyToMany(x => x.TimeSlots).Cascade.None().Not.LazyLoad();
        }
    }
}
