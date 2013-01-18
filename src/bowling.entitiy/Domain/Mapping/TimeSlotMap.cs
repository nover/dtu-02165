using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain.Mapping
{
    class TimeSlotMap  : ClassMap<TimeSlot>
    {
        public TimeSlotMap()
        {
            Id(x => x.Id);
            Map(x => x.Start);
            Map(x => x.End);
            HasManyToMany(x => x.Reservations).Inverse().Cascade.None();

        }
    }
}
