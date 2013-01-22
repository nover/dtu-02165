using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Types
{
    public class TimeSlotType
    {
        public int Id { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}
