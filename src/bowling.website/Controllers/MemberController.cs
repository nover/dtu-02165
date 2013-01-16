using AutoMapper;
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
	public class MemberController : BootstrapBaseController
	{
		//
		// GET: /Member/

		public ActionResult Index()
		{
			return View();
		}

        public ActionResult MyProfile()
        {
            var member = Session["member"] as MemberInputModel;
            if (member != null)
            {
                View(member);
            }

            return View(new MemberInputModel());
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

                var jsonClient = this.CurrentAPIClient;
                Members request = Mapper.Map<MemberInputModel, Members>(model);

                var response = jsonClient.Post<MembersResponse>("/members", request);
                var member = Mapper.Map<MembersResponse, MemberInputModel>(response);
                Session.Add("member", member);

				Success("User created successfully!");
                
				return RedirectToAction("MyProfile");
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
