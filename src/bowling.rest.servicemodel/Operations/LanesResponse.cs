using Bowling.Rest.Service.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    public class LanesResponse
    {
        public IList<LaneType> Lanes {get; set;}

        public LanesResponse()
        {
            this.Lanes = new List<LaneType>();
        }
    }
}
