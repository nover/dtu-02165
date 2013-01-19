using AutoMapper;
using bowling.administration.website.Models;
using Bowling.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bowling.administration.website
{
	public static class AutomapperConfig
	{
		public static void CreateMappings()
		{
			Mapper.CreateMap<TimeSlot, TimeSlotInputModel>();
			Mapper.CreateMap<TimeSlotInputModel, TimeSlot>();
			Mapper.CreateMap<Lane, LaneInputModel>();
			Mapper.CreateMap<LaneInputModel, Lane>();
		}
	}
}