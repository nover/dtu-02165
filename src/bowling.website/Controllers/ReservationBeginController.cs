using BootstrapMvcSample.Controllers;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Web.CustomerSite.Models;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
					var response = this.CurrentAPIClient.Get<ReservationPossibleResponse>(
						String.Format(
							"/reservation/possible?NumberOfPlayers={0}&PlayAt={1}&TimeOfDay={2}HowManyHours={3}",
							model.NumberOfPeople,
							model.PlayAt,
							model.StartPlayingAt,
							model.HowManyHours
						));


				}
				catch (WebServiceException ex)
				{
					Error("Awww, something terrible happened. The Leprechauns are working on the problem se please try again later.");
					return View(model);
				}

                throw new NotImplementedException("Proceeding to next step is not supported yet");
            }

            Error("Darn, Something in your input was not correct. Please check it.");
            return View(model);
        }
    }
}
