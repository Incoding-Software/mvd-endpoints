namespace MvdEndPoint.UI.Controllers
{
    #region << Using >>

    using Incoding.Endpoint.Infrastructure;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class DispatcherController : DispatcherControllerBase
    {
        #region Constructors

        public DispatcherController()
                : base(new[]
                       {
                               typeof(Bootstrapper).Assembly,                                
                       }) { }

        #endregion
    }
}