using ServiceStack.ServiceInterface;
using System;
using Bowling.Rest.Service.Model.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SharpLite.Domain.DataInterfaces;
using Bowling.Entity.Domain;
using Bowling.Entity.Queries;
using AutoMapper;
using Bowling.Rest.Service.Model.Types;
using Microsoft.Practices.ServiceLocation;
namespace Bowling.Rest.Service.Interface.Services
{
    class MembersLoginService : RestServiceBase<MembersLogin>
    {

        public override object OnGet(MembersLogin request)
        {
            var repository = ServiceLocator.Current.GetInstance<IRepository<Member>>();
             var member = repository.GetAll().FindByEmailAndPassword(request.Email, request.Password);

            MembersLoginResponse response = new MembersLoginResponse();
            response.IsAuthenticated = false;

            //if the user was not authenticated
            if (member == default(Member))
            {
                return response;
            }
            response.IsAuthenticated = true;
            response.Member = Mapper.Map<Member, MemberType>(member);
            return response;
        }
    }
}
