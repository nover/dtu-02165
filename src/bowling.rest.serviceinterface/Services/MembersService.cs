using AutoMapper;
using Bowling.Entity.Domain;
using Bowling.Rest.Service.Model.Operations;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Services
{
    class MembersService : RestServiceBase<Members>
    {
        public override object OnGet(Members request)
        {
            if (request.Email == null)
            {
                return this.OnGetCollection(request);
            }
            else
            {
                return this.OnGetItem(request);
            }
        }

        private MembersResponse OnGetItem(Members request)
        {
            throw new NotImplementedException();
        }

        private MembersResponse OnGetCollection(Members request)
        {
            throw new NotImplementedException();
        }


        public override object OnPost(Members request)
        {
            //return base.OnPost(request);
            /*Add the code for inserting the new memeber into the appropriate 
             database table*/

            Member member = Mapper.Map<Members, Member>(request);
            

            throw new NotImplementedException();
        }
    }
}
