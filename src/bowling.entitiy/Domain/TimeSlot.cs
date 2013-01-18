using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain 
{
    public class TimeSlot : SharpLite.Domain.Entity
    {
        public virtual  DateTime From { get; set; }
        public virtual DateTime To { get; set; }
    }
}
