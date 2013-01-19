﻿using BootstrapMvcSample.Controllers;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Web.CustomerSite.Models;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Bowling.Rest.Service.Model.Types;

namespace Bowling.Web.CustomerSite.Controllers
{
	public class ReservationController : BootstrapBaseController
	{
		//
		// GET: /ReservationBegin/

		public ActionResult Index()
		{
			return View(new ReservationInputModel());
		}

		[HttpPost]
		public ActionResult Index(ReservationInputModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var request = new ReservationPossible();
					request.Reservation = Mapper.Map<ReservationType>(model);

					var response = this.CurrentAPIClient.Post<ReservationPossibleResponse>(
						"/reservation/possible", 
						request);

					if (response.IsPossible)
					{
						this.CurrentReservation = Mapper.Map<ReservationType>(model);
						return RedirectToAction("ContactInfo");
					}

					Information("Unfortunately this particular reservation is not possible, but please look at the page for suggestions.");
					return View(model);
					
				}
				catch (WebServiceException ex)
				{
					Error("Awww, something terrible happened. The Leprechauns are working on the problem se please try again later. Techical details: " + ex.Message);
					return View(model);
				}

			}

			Error("Darn, Something in your input was not correct. Please check it.");
			return View(model);
		}

		public ActionResult ContactInfo()
		{
			return View(new ReservationContactInfoInputModel());
		}

		[HttpPost]
		public ActionResult ContactInfo(ReservationContactInfoInputModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					this.CurrentReservation.Name = model.Name;
					this.CurrentReservation.PhoneNumber = model.Cellphone;

					var request = new Reservations()
					{
						Reservation = this.CurrentReservation
					};

					this.CurrentAPIClient.Post<ReservationsResponse>("/reservation", request);

					Success("Your reservation was created successfully");

					return RedirectToAction("Complete");
				}
				catch (WebServiceException ex)
				{
					Error("Something bad happened while trying to save your reservation, please try again");
					return View(model);
				}
			}

			Error("There is an error in your data");
			return View(model);
		}

		public ActionResult Complete()
		{
			throw new NotImplementedException();
		}
	}
}
