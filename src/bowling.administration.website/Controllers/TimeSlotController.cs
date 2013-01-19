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
		private IRepository<TimeSlot> repos;
		public TimeSlotController(IRepository<TimeSlot> repos)
		{
			this.repos = repos;
		}
        //
        // GET: /TimeSlot/

        public ActionResult Index()
        {
			var slots = Mapper.Map<List<TimeSlot>, List<TimeSlotInputModel>>( this.repos.GetAll().ToList());

            return View(slots);
        }

		[HttpPost]
		public ActionResult Create(TimeSlotInputModel model)
		{
			if (ModelState.IsValid)
			{
				var toSave = Mapper.Map<TimeSlot>(model);
				try
				{
					this.repos.SaveOrUpdate(toSave);
					this.repos.DbContext.CommitChanges();
				}
				catch (Exception ex)
				{
					Error("Something went horribly wrong while saving your changes<br/>Technical stuff: " + ex.Message);
					return View(model);
				}

				Success("Your new time slot was saved");
				return RedirectToAction("Index");
			}

			Error("Darn, your input was incorrect. Please try again");
			return View(model);
		}

		public ActionResult Create()
		{
			return View(new TimeSlotInputModel());
		}
	}
}
