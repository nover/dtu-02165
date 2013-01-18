using Bowling.Web.CustomerSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bowling.Web.CustomerSite.Controllers
{
    public class ReservationBeginController : Controller
    {
        //
        // GET: /ReservationBegin/

        public ActionResult Index()
        {
            return View(new ReservationBeginInputModel());
        }

    }
}
