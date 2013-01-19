using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain 
{
    public class Lane : SharpLite.Domain.Entity
    {
        public virtual string Name { get; set; }

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
