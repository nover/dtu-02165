using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain 
{
    public class TimeSlot : SharpLite.Domain.Entity
    {
        //http://stackoverflow.com/questions/6278394/type-to-store-time-in-c-sharp-and-corresponding-type-in-t-sql

        public virtual  TimeSpan Start { get; set; }
        public virtual TimeSpan End { get; set; }

        private IList<Reservation> reservations = new List<Reservation>();

        public virtual IList<Reservation> Reservations
        {
            get
            {
                return this.reservations;
            }
            protected set
            {
                this.reservations = value;
            }
        }
    }
}
