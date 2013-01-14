using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Validation
{
    interface IEmailValidator
    {
        bool IsValid(string email);
    }
}
