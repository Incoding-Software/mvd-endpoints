namespace MvdEndPoint.UI
{
    #region << Using >>

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                            name: "Default", 
                            url: "{controller}/{action}", 
                            defaults: new { controller = "Home", action = "Index" }
                    );
        }
    }
}