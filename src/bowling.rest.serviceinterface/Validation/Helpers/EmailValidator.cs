using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Validation
{
    class EmailValidator : IEmailValidator
    {
        public bool IsValid(string email)
        {
            return false;
        }
    }
}
