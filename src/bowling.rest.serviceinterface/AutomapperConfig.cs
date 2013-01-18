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
			Mapper.CreateMap<Reservation, ReservationType>();
		}
	}
}
