﻿using Bowling.Rest.Service.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
    public class MembersLoginResponse
    {
        public bool IsAuthenticated { get; set; }

        public MemberType Member { get; set; }
    }
}
