using BootstrapMvcSample.Controllers;
using Bowling.Web.CustomerSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bowling.Web.CustomerSite.Controllers
{
	public class MemberController : BootstrapBaseController
	{
		//
		// GET: /Member/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult SignUp()
		{
			return View(new MemberInputModel());
		}

		[HttpPost]
		public ActionResult SignUp(MemberInputModel model)
		{
			if (ModelState.IsValid)
			{
				throw new NotImplementedException("Model state is valid, but commit is not implemented yet");
				//Success("Your information was saved!");
				//return RedirectToAction("Index");
			}

			Error("there were some errors in your form.");
			return View(model);
		}

		public ActionResult Login()
		{
			throw new NotImplementedException();
		}

	}
}
