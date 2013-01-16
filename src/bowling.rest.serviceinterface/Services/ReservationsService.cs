using Bowling.Rest.Service.Model.Operations;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Services
{
    public class ReservationsService : RestServiceBase<Reservations>
    {
        public override object OnPost(Reservations request)
        {
            throw new NotImplementedException();
        }

    }
}
