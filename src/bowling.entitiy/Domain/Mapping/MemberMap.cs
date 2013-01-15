using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain.Mapping
{
    class MemberMap : ClassMap<Member>
    {
        public MemberMap()
        {
            Id(x => x.Id);
            Map(x => x.Email);
            Map(x => x.Name);
        }
    }
}
