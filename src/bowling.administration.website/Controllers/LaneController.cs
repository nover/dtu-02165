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
	public class LaneController : BootstrapBaseController
	{
		private IRepository<Lane> repos;

		public LaneController(IRepository<Lane> repos)
		{
			this.repos = repos;
		}
		//
		// GET: /Lane/

		public ActionResult Index()
		{
			var lanes = Mapper.Map<List<Lane>, List<LaneInputModel>>( this.repos.GetAll().ToList() );
			return View(lanes);
		}

		[HttpPost]
		public ActionResult Create(LaneInputModel model)
		{
			if (ModelState.IsValid)
			{
				var toSave = Mapper.Map<Lane>(model);
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

				Success("Your new lane was saved");
				return RedirectToAction("Index");
			}

			Error("Darn, your input was incorrect. Please try again");
			return View(model);
		}

		public ActionResult Create()
		{
			return View(new LaneInputModel());
		}

		public ActionResult Delete(int id)
		{
			try
			{
				this.repos.Delete(this.repos.Get(id));
				Information("The lane was successfully removed");

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Error("Something went horribly wrong while removing the lane<br/>Techincal stuff: " + ex.Message);
				return RedirectToAction(Request.UrlReferrer.LocalPath);
			}
		}

		public ActionResult Edit(int id)
		{
			var model = Mapper.Map<LaneInputModel>(this.repos.Get(id));
			return View("Create", model);
		}

		[HttpPost]
		public ActionResult Edit(LaneInputModel model, int id)
		{
			if (ModelState.IsValid)
			{
				var timeslot = Mapper.Map<Lane>(model);
				this.repos.SaveOrUpdate(timeslot);
				this.repos.DbContext.CommitChanges();

				Success("Timeslot was successfully updated");
				return RedirectToAction("Index");
			}
			return View("Create", model);
		}

		public ActionResult Details(int id)
		{
			var model = Mapper.Map<LaneInputModel>(this.repos.Get(id));
			return View(model);
		}

	}
}
