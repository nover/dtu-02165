using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    public class ReservationsResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String Status { get; set; }
    }
}
