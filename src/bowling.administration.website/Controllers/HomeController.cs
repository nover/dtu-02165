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
            throw new NotImplementedException(); 
            //var date = DateTime.Now;
            //var reservations = client.Get<ReservationsResponse>(String.Format("/reservation?Date={0}", date.ToShortDateString()));

            //var dataList = new List<DataCarrier>();
            //foreach (var time in timeSlots.TimeSlots)
            //{
            //    var data = new DataCarrier();
            //    data.TimeTag = String.Format("{0} - {1}", time.Start, time.End);

            //    var reservationsForTimeSlot = from y in reservations.ReservationList
            //                                  where y.TimeSlots.Contains(time)
            //                                  select y;
            //    foreach (var lane in lanes.Lanes)
            //    {
            //        data.LanesForTime.Add(new Tuple<LaneType, ReservationType>(lane, null));
            //    }
            //    dataList.Add(data);
            //}
            

            //return Json(new
            //{
            //    Schedule = dataList
            //}, JsonRequestBehavior.AllowGet);
        }



        private class DataCarrier
        {
            public string TimeTag { get; set; }
            public List<Tuple<LaneType, ReservationType>> LanesForTime { get; set; }

            public DataCarrier()
            {
                this.LanesForTime = new List<Tuple<LaneType, ReservationType>>();
            }
        }
    }
}
