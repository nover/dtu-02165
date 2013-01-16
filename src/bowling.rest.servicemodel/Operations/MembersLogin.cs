using Bowling.Rest.Service.Model.Types;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    [Route("/memberslogin")]
    public class MembersLogin
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
