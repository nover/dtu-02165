using AutoMapper;
using Bowling.Entity.Domain;
using Bowling.Rest.Service.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface
{
	public class AutomapperConfig
	{
		public static void CreateMapping()
		{

			// define the map rules between the Members DTO(coming from the network) and the Member entity (corresponding to a row of the Member's table)
			Mapper.CreateMap<MemberType, Member>();
			Mapper.CreateMap<Member, MemberType>();
            Mapper.CreateMap<Reservation, ReservationType>()
                .ForMember(dest => dest.HowManyHours, opt => opt.MapFrom(src => src.TimeSlots.Last().End.Hours - src.TimeSlots.First().Start.Hours))
                .ForMember(dest => dest.NumberOfPlayers, opt => opt.MapFrom(src => src.NumberOfPlayers))
                .ForMember(dest => dest.PlayAt, opt => opt.MapFrom(src => src.PlayAt))
                .ForMember(dest => dest.TimeOfDay, opt => opt.MapFrom(src => src.TimeSlots[0].Start));
            Mapper.CreateMap<TimeSlot, TimeSlotType>();
            Mapper.CreateMap<Lane, LaneType>();

            Mapper.AssertConfigurationIsValid();
		}
	}
}
