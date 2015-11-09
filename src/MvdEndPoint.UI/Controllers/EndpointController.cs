namespace MvdEndPoint.UI.Controllers
{
    using System.Web.Mvc;    
    using Incoding.MvcContrib;

    public class EndpointController : IncControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}