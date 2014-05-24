using System;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(MvdEndPoint.UI.App_Start.IncodingStart), "PreStart")]

namespace MvdEndPoint.UI.App_Start {
    using MvdEndPoint.Domain;
    using MvdEndPoint.UI.Controllers;

    public static class IncodingStart {
        public static void PreStart() {
            Bootstrapper.Start();
            new DispatcherController(); // init routes
        }
    }
}