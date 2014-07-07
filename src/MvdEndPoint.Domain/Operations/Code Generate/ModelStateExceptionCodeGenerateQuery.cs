namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class ModelStateExceptionCodeGenerateQuery : QueryBase<string>
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