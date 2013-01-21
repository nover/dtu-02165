using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    [Route("/members/exist")]
    public class MemberExists
    {
        public string Email { get; set; }
    }
}
