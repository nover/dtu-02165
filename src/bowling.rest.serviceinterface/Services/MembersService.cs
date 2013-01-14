using Bowling.Rest.Service.Model.Operations;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Services
{
    class MembersService : RestServiceBase<MembersRequest>
    {
        public override object OnGet(MembersRequest request)
        {
            if (request.Id == null)
            {
                return this.OnGetCollection(request);
            }
            else
            {
                return this.OnGetItem(request);
            }
        }

        private MembersResponse OnGetItem(MembersRequest request)
        {
            throw new NotImplementedException();
        }

        private MembersResponse OnGetCollection(MembersRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
