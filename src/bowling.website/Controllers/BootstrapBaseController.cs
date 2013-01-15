using System.Web.Mvc;
using BootstrapSupport;
using ServiceStack.ServiceClient.Web;

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
    }
}
