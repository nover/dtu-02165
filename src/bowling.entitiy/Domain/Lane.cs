using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain 
{
    public class Lane : SharpLite.Domain.Entity
    {
        public int Number { get; set; }
    }
}
