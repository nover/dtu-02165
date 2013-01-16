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
namespace Bowling.Rest.Service.Interface.Services
{
    class MembersLoginService : RestServiceBase<MembersLogin>
    {
        
        public override object OnGet(MembersLogin request)
        {
            var repository = DependencyResolver.Current.GetService<IRepository<Member>>();
            var member = repository.GetAll().FindByEmailAndPassword(request.Email, request.Password);

            //if the user was not authenticated
            if (member == default(Member))
            {
                throw new ArgumentException("Login is incorrect");
            }
            else
            {
                MembersLoginResponse response = Mapper.Map<Member, MembersLoginResponse>(member);
                return response;
            } 
     
        }
    }
}
