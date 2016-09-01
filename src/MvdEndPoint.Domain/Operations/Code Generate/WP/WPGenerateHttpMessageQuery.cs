namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.WP;

    #endregion

    public class WPGenerateHttpMessageQuery : QueryBase<string>
    {
        protected override string ExecuteResult()
        {
            var engine = new WP_HttpMessageBase();
            engine.Session = new Dictionary<string, object>()
                             {
                                     { "Url", Url },
                                     { "Namespace", Namespace },
                                     { "IsNotifyPropertyChanged", true },
                             };
            engine.Initialize();
            return engine.TransformText();
        }

        #region Properties

        public string Url { get; set; }

        public string Namespace { get; set; }

        #endregion
    }
}