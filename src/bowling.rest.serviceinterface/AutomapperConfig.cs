using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface
{
    class AutomapperConfig
    {
        public static void CreateMapping()
        {

            // define the map rules between the Members DTO(coming from the network) and the Member entity (corresponding to a row of the Member's table)
            Mapper.CreateMap<Bowling.Rest.Service.Model.Operations.Members, Bowling.Entity.Domain.Member>();
            Mapper.CreateMap<Bowling.Entity.Domain.Member,Bowling.Rest.Service.Model.Operations.MembersResponse>();
        }
    }
}
