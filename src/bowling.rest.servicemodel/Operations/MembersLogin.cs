using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    [Route("/members_login")]
    class MembersLogin
    {
        public String Email { get; set; }
        public String password { get; set; }
    }
}
