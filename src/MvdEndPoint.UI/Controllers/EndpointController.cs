namespace MvdEndPoint.UI.Controllers
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MvcContrib;

    #endregion

    public class EndpointController : IncControllerBase
    {
        public ActionResult Index()
        {
            dispatcher.Push(new SyncEndpointCommand());
            return View();
        }
    }
}