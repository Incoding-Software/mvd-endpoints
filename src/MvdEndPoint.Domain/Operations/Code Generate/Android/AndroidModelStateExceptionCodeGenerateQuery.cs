namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Android;

    #endregion

    public class AndroidModelStateExceptionCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public string Namespace { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var template = new Android_ModelStateException();
            template.Session = new Dictionary<string, object>
                                   {
                                           { "Namespace", Namespace }
                                   };
            template.Initialize();
            return template.TransformText();
        }
    }
}