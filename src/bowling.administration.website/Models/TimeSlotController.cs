using AutoMapper;
using BootstrapMvcSample.Controllers;
using bowling.administration.website.Models;
using Bowling.Entity.Domain;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bowling.administration.website.Controllers
{
    public class TimeSlotController : BootstrapBaseController
    {
        //
        // GET: /TimeSlot/

        public ActionResult Index()
        {
			var repos = DependencyResolver.Current.GetService<IRepository<TimeSlot>>();
			var slots = Mapper.Map<List<TimeSlot>, List<TimeSlotInputModel>>( repos.GetAll().ToList());

            return View(slots);
        }


		public ActionResult Create()
		{
			return View(new TimeSlotInputModel());
		}
	}
}
