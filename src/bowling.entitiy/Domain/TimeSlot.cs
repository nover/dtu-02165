using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain 
{
    public class TimeSlot : SharpLite.Domain.Entity
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
