using Bowling.Rest.Service.Model.Types;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    [Route("/reservation")]
    public class Reservations
    {
        public DateTime PlayAt { get; set; }
        public int NumOfPlayers { get; set; }
        public String Name { get; set; }
        public String PhoneNumber { get; set; }
        public MemberType member { get; set; }
    }
    
}
