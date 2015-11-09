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
                : base(new[]
                       {
                               typeof(Bootstrapper).Assembly, 
                               typeof(Main.Domain.Bootstrapper).Assembly
                       }) { }

        #endregion
    }
}