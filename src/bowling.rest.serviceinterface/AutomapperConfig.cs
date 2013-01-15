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
            Mapper.CreateMap<Bowling.Rest.Service.Model.Operations.Members, Bowling.Entity.Domain.Member>();
        }
    }
}
