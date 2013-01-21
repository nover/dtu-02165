using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Types
{
    public class MemberType
    {
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }
        public String DialCode { get; set; }
        public String CellPhone { get; set; }
        public int DefaultNumOfPlayers { get; set; }
        public bool? ReceiveNewsLetter { get; set; }
    }
}
