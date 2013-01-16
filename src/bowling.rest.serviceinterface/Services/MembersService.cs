using AutoMapper;
using Bowling.Entity.Domain;
using Bowling.Rest.Service.Model.Operations;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpLite.NHibernateProvider;
using System.Web.Mvc;
using SharpLite.Domain.DataInterfaces;
using Microsoft.Practices.ServiceLocation;

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
            // it maps the Members instance coming from the ServiceStack to the Member Entity, where member entity resembles a
            // row in the Member table
            Member member = Mapper.Map<Members, Member>(request);

            //the repository describes a manager that handles a certain table, Member table in that case
            var repository = ServiceLocator.Current.GetInstance<IRepository<Member>>();
            //use the Member table manager to insert a new Member instance
            repository.DbContext.BeginTransaction();
            repository.SaveOrUpdate(member);
            repository.DbContext.CommitTransaction();

            // TODO: Create return data to website
            MembersResponse response = Mapper.Map<Member, MembersResponse>(member);
            return response;
        }
    }
}
