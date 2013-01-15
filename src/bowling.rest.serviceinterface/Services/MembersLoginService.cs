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
namespace Bowling.Rest.Service.Interface.Services
{
    class MembersLoginService : RestServiceBase<MembersLogin>
    {
        
        public override object OnGet(MembersLogin request)
        {
            if (request.Email == null)
                return this.OnGetNullEmail(request);
            else
                return this.OnGetValidCredentials(request);           
        }

        private MembersLoginResponse OnGetNullEmail(MembersLogin request)
        {
            throw new NotImplementedException();
        }


        private MembersLoginResponse OnGetValidCredentials(MembersLogin request) 
        {
            var repository = DependencyResolver.Current.GetService<IRepository<Member>>();
            var member = repository.GetAll().FindByEmailAndPassword(request.Email, request.Password);

            if (member == default(Member))
            {
                // TODO: was NOT autenticated
            }
            else
            {
                // TODO: was authenticated
            }

            throw new NotImplementedException();
        }

    }
}
