using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain
{
    public class Member : SharpLite.Domain.Entity
    {
        public virtual String Email { get; set; }
        public virtual String Password { get; set; }
        public virtual String Name { get; set; }
        public virtual String Title { get; set; }
        public virtual String DialCode { get; set; }
        public virtual String CellPhone { get; set; }
        public virtual int DefaultNumOfPlayers { get; set; }
        public virtual bool ReceiveNewsLetter { get; set; }
    }
}
