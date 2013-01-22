using Bowling.Rest.Service.Model.Operations;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bowling.administration.website.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Reservations()
        {
            var client = new JsonServiceClient("http://localhost:24920/");
            var lanes = client.Get<LanesResponse>("/lanes");

            var timeSlots = client.Get<TimeSlotsResponse>("/timeslots");

            var date = DateTime.Now;
            var reservations = client.Get<ReservationsResponse>(String.Format("/reservation?Date={0}", date.ToShortDateString()));


            // TODO: Call the Service API to get the reservations for Today
            // TODO: convert the response tree into JSON by using: return JSON(TheObject,JsonRequestBehavior.AllowGet); 
            throw new NotImplementedException();
        }

    }
}
