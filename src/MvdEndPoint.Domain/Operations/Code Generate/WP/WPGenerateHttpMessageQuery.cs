namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate.WP;

    #endregion

    public class WPGenerateHttpMessageQuery : QueryBase<string>
    {
        #region Properties

        public string Url { get; set; }

        public string Namespace { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var engine = new WP_HttpMessageBase();
            engine.Session = new Dictionary<string, object>()
                             {
                                     { "Url", Url }, 
                                     { "Namespace", Namespace }
                             };
            engine.Initialize();
            return engine.TransformText();
        }
    }
}