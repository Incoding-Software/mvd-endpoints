namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class JsonModelStateDataCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public string Namespace { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var androidClass = new Android_JsonModelStateData();
            androidClass.Session = new Dictionary<string, object>
                                       {
                                               { "Namespace", Namespace },
                                       };
            androidClass.Initialize();
            return androidClass.TransformText();
        }
    }
}