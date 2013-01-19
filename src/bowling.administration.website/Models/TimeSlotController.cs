using BootstrapMvcSample.Controllers;
using bowling.administration.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bowling.administration.website.Controllers
{
    public class TimeSlotController : BootstrapBaseController
    {
        //
        // GET: /TimeSlot/

        public ActionResult Index()
        {
            return View();
        }


		public ActionResult Create()
		{
			return View(new TimeSlotInputModel());
		}
	}
}
