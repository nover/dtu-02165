using AutoMapper;
using Bowling.Entity.Domain;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Model.Types;
using Microsoft.Practices.ServiceLocation;
using ServiceStack.ServiceInterface;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Services
{
    public class LanesService : RestServiceBase<Lanes>
    {

        public override object OnGet(Lanes request)
        {
            if (request.Id == null)
            {
                return this.OnGetCollection();
            }
            else
            {
                throw new NotSupportedException("GET of single lane is not supported");
            }
        }

        private LanesResponse OnGetCollection()
        {
            var laneRepos = ServiceLocator.Current.GetInstance<IRepository<Lane>>();
            var response = new LanesResponse();
            response.Lanes = Mapper.Map<IList<Lane>, List<LaneType>>(laneRepos.GetAll().ToList());

            return response;
        }
    }
}
