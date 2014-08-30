namespace MvdEndPoint.UI.Controllers
{
    #region << Using >>

    using Incoding.MvcContrib.MVD;
    using InstaLine.Domain;

    #endregion

    public class DispatcherController : DispatcherControllerBase
    {
        #region Constructors

        public DispatcherController()
                : base(new[]
                           {
                                   typeof(Bootstrapper).Assembly,
                                   typeof(Domain.Bootstrapper).Assembly
                           }) { }

        #endregion
    }
}