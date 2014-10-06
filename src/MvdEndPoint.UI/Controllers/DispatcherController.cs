namespace MvdEndPoint.UI.Controllers
{
    #region << Using >>

    using EDating.Domain;
    using Incoding.MvcContrib.MVD;
    

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