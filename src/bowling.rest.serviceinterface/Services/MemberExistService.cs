using Bowling.Entity.Domain;
using Bowling.Rest.Service.Model.Operations;
using Microsoft.Practices.ServiceLocation;
using ServiceStack.ServiceInterface;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bowling.Entity.Queries;
using AutoMapper;
using Bowling.Rest.Service.Model.Types;
namespace Bowling.Rest.Service.Interface.Services
{
    public class MemberExistService: RestServiceBase<MemberExists>
    {
        public override object OnGet(MemberExists request)
        {
            var repository = ServiceLocator.Current.GetInstance<IRepository<Member>>();
            var member = repository.GetAll().FindMemberByEmail(request.Email);

            // TODO: Create return data to website
            MemberExistsResponse response = new MemberExistsResponse();
            response.DoesExist = false;
            //if the user already exists
            if (member == default(Member))
            {
                return response;
            }

            response.DoesExist = true;
            return response;
        }
    }
}
