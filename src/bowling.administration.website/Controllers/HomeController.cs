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
            // TODO: Call the Service API to get the reservations for Today
            // TODO: convert the response tree into JSON by using: return JSON(TheObject,JsonRequestBehavior.AllowGet); 
            throw new NotImplementedException();
        }

    }
}
