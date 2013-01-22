using Bowling.Rest.Service.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    public class TimeSlotsResponse
    {
        public IList<TimeSlotType> TimeSlots { get; set; }

        public TimeSlotsResponse()
        {
            this.TimeSlots = new List<TimeSlotType>();
        }
    }
}
