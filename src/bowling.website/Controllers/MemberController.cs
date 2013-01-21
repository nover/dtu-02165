using AutoMapper;
using BootstrapMvcSample.Controllers;
using Bowling.Rest.Service.Model.Operations;
using Bowling.Rest.Service.Model.Types;
using Bowling.Web.CustomerSite.Models;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            //this.
            var member = this.LoggedInMember;
            if (member != null)
            {
                return View(member);
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
                var emailResponse = jsonClient.Get<MemberExistsResponse>(
                    String.Format(
                        "/members/exist?email={0}",
                        model.Email
                        ));

                if (emailResponse.DoesExist)
                {
                    Error("Sorry, the email is already used by another member!");
                    return View(model);
                }


                Members request = new Members();
                request.Member = Mapper.Map<MemberInputModel, MemberType>(model);

                var response = jsonClient.Post<MembersResponse>("/members", request);
                var member = Mapper.Map<MemberType, MemberInputModel>(response.Member);
                // if we get here, all is OK
                FormsAuthentication.SetAuthCookie(model.Email, false);
                this.LoggedInMember = member;

                Success("User created successfully!");

                return RedirectToAction("MyProfile");
            }

            Error("there were some errors in your form.");
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(MemberLoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                // Authenticate with API
                var jsonClient = this.CurrentAPIClient;

                try
                {
                    MembersLoginResponse response = jsonClient.Get<MembersLoginResponse>(
                        String.Format(
                            "/memberslogin?Email={0}&password={1}",
                            model.Email,
                            model.Password));

                    if (!response.IsAuthenticated)
                    {
                        Error("You didn't type a valid email/password combination, please try again");
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    // if we get here, all is OK
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    this.LoggedInMember = Mapper.Map<MemberInputModel>(response.Member);

                    Success("You are now logged in!");
                    return Redirect("MyProfile");
                }
                catch (WebServiceException ex)
                {
                    Error("Oh no, the API went away - login failed. Please try again in a few minutes...");
                    return RedirectToAction("Index", "Home");
                }
            }


            return View(model);
            throw new NotImplementedException();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            this.LoggedInMember = null;

            Information("Successfully logged out");
            return RedirectToAction("Index", "Home");
        }

    }
}
