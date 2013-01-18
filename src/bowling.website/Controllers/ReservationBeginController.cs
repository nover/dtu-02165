using BootstrapMvcSample.Controllers;
using Bowling.Web.CustomerSite.Models;
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
                throw new NotImplementedException("Proceeding to next step is not supported yet");
            }

            Error("Darn, Something in your input was not correct. Please check it.");
            return View(model);
        }
    }
}
