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
	public class ReservationBeginController : BootstrapBaseController
	{
		//
		// GET: /ReservationBegin/

		public ActionResult Index()
		{
			return View(new ReservationBeginInputModel());
		}

		[HttpPost]
		public ActionResult Index(ReservationBeginInputModel model)
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
						return RedirectToAction("Index", "ReservationContinue");
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
	}
}
