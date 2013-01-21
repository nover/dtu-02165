﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Types
{
    public class MemberType
    {
        public int Id { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public MemberTitle Title { get; set; }
        public String DialCode { get; set; }
        public int CellPhone { get; set; }
        public int DefaultNumberOfPlayers { get; set; }
        public bool? ReceiveNewsLetter { get; set; }
    }
}
