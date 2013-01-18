using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain.Mapping
{
    class LaneMap : ClassMap<Lane>
    {

        public LaneMap() 
        {
            Id(x => x.Id);
            Map(x => x.Number);
            HasManyToMany(x => x.Reservations).Inverse();

        }
    }
}
