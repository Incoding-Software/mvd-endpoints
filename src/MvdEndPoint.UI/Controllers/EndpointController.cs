namespace MvdEndPoint.UI.Controllers
{
    using System.Web.Mvc;
    using Incoding.MvcContrib;

    public class EndpointController : IncControllerBase
    {
        #region Api Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}