using System.Web.Mvc;
using BootstrapSupport;
using ServiceStack.ServiceClient.Web;
using Bowling.Rest.Service.Model.Types;
using Bowling.Web.CustomerSite.Models;

namespace BootstrapMvcSample.Controllers
{
    public class BootstrapBaseController: Controller
    {
        public void Attention(string message)
        {
            TempData.Add(Alerts.ATTENTION, message);
        }

        public void Success(string message)
        {
            TempData.Add(Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            TempData.Add(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            TempData.Add(Alerts.ERROR, message);
        }

        public ServiceClientBase CurrentAPIClient
        {
            get
            {
                ServiceClientBase instance = Session["apiclient"] as JsonServiceClient;

                if (instance == null)
                {
                    instance = new JsonServiceClient("http://localhost:24920/");
                    Session["apiclient"] = instance;
                }

                return instance;
            }
        }

		public ReservationType CurrentReservation
		{
			get
			{
				ReservationType resv = Session["reservation"] as ReservationType;
				if (resv == null)
				{
					resv = new ReservationType();
					Session["reservation"] = resv;
				}

				return resv;
			}
			set
			{
				Session["reservation"] = value;
			}
		}

        public MemberInputModel LoggedInMember
        {
            get
            {
                MemberInputModel instance = Session["member"] as MemberInputModel;
                if (instance == null)
                {
                    instance = new MemberInputModel();
                    Session["member"] = instance;
                }

                return instance;
            }
            set
            {
                Session["member"] = value;
            }
        }
    }
}
