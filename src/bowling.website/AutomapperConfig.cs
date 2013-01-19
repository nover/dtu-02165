using AutoMapper;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Model.Types;
using Bowling.Web.CustomerSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bowling.Web.CustomerSite
{
    public class AutomapperConfig
    {
        public static void ApplyConfiguration()
        {
            Mapper.CreateMap<MemberInputModel, MemberType>();
            Mapper.CreateMap<MemberType, MemberInputModel>();
			Mapper.CreateMap<ReservationBeginInputModel, ReservationType>()
				.ForMember(dest => dest.TimeOfDay, cfg => cfg.MapFrom(src => src.StartPlayingAt));
        }
    }
}