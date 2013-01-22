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
    public class TimeSlotsService : RestServiceBase<TimeSlots>
    {
        public override object OnGet(TimeSlots request)
        {
            if (request.Id == null)
            {
                return this.OnGetCollection();
            }
            else
            {
                throw new NotSupportedException("GET for specific timeslot is not supported yet");
            }
        }

        private TimeSlotsResponse OnGetCollection()
        {
            var timeSlotRepos = ServiceLocator.Current.GetInstance<IRepository<TimeSlot>>();
            var response = new TimeSlotsResponse();
            response.TimeSlots = Mapper.Map<IList<TimeSlot>, List<TimeSlotType>>(timeSlotRepos.GetAll().ToList());

            return response;
        }
    }
}
