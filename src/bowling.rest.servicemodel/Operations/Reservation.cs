using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    [Route("/reservation")]
    public class Reservation
    {
        public DateTime Date { get; set; }
        public String Status { get; set; }
    }
}
