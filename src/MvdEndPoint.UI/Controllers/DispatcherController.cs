namespace MvdEndPoint.UI.Controllers
{
    #region << Using >>

    using Incoding.MvcContrib.MVD;
    using MvdEndPoint.Domain;

    #endregion

    public class DispatcherController : DispatcherControllerBase
    {
        #region Constructors

        public DispatcherController()
                : base(typeof(Bootstrapper).Assembly) { }

        #endregion
    }
}