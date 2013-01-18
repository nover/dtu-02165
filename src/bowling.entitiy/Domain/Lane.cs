using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain 
{
    public class Lane : SharpLite.Domain.Entity
    {
        public virtual int Number { get; set; }
        public virtual IList<Reservation> Reservations { get; set; }


    }
}
