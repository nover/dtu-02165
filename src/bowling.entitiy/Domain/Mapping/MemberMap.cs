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
            Map(x => x.Email).Not.Nullable().Unique();
            Map(x => x.Password);
            Map(x => x.Name);
            Map(x => x.Title);
            Map(x => x.DialCode);
            Map(x => x.CellPhone);
            Map(x => x.DefaultNumberOfPlayers);
            Map(x => x.ReceiveNewsLetter);
            
            
        }
    }
}
