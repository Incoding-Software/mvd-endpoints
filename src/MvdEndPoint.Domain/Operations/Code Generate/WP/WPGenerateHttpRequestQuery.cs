namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate.WP;

    #endregion

    public class WPGenerateHttpRequestQuery : QueryBase<string>
    {
        public string Url { get; set; }

        protected override string ExecuteResult()
        {
            var engine = new Wp_HttpRequest();
            engine.Session = new Dictionary<string, object>()
                             {
                                     { "Url", Url }
                             };
            engine.Initialize();
            return engine.TransformText();
        }
    }
}