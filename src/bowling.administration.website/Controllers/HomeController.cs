using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Model.Types;
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
        private JsonServiceClient client;

        public HomeController()
        {
            this.client = new JsonServiceClient("http://localhost:24920/");
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult AjaxGetLanesAndTimeSlots()
        {
            var lanes = client.Get<LanesResponse>("/lanes");
            var timeSlots = client.Get<TimeSlotsResponse>("/timeslots");
            timeSlots.TimeSlots = timeSlots.TimeSlots.OrderByDescending(x => x.End).ToList();


            return Json(new
            {
                Lanes = lanes.Lanes,
                TimeSlots = timeSlots.TimeSlots
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult AjaxGetReservations()
        {
            var date = DateTime.Now;
            var reservations = client.Get<ReservationsResponse>(String.Format("/reservation?Date={0}", date.ToShortDateString()));

            var dataList = new List<DataCarrier>();

            foreach (var r in reservations.ReservationList)
            {
                foreach (var slot in r.TimeSlots)
                {
                    foreach (var lane in r.Lanes)
                    {
                        var name = r.Name;
                        if (name.Length > 8)
                        {
                            name = name.Substring(0, 8) + "...";
                        }
                        dataList.Add(new DataCarrier
                        {
                            LaneId = lane.Id,
                            Name = name,
                            ReservationId = r.Id,
                            ReservationStatus = r.Status,
                            TimeSlotId = slot.Id
                        });
                    }
                }
            }
            
            return Json(new
            {
                Reservations = dataList
            }, JsonRequestBehavior.AllowGet);
        }

        private class DataCarrier
        {
            public string Name { get; set; }

            public int ReservationId { get; set; }

            public int LaneId { get; set; }

            public int TimeSlotId { get; set; }

            public ReservationStatusType ReservationStatus { get; set; }
        }
    }
}
