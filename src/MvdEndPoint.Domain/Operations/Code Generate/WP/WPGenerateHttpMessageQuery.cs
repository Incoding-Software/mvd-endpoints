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
                                     { "Namespace", Namespace },                                     
                             };
            engine.Initialize();
            return engine.TransformText();
        }

        #region Properties

        
        public string Namespace { get; set; }

        #endregion
    }
}