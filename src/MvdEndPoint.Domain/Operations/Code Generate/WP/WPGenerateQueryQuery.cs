namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.WP;

    #endregion

    public class WPGenerateQueryQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var engine = new WP_Query();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery() { Type = Type });
            var responseOfType = Type.BaseType.GetGenericArguments()[0];
            engine.Session = new Dictionary<string, object>()
                             {
                                     { "Meta", meta },
                                     { "Properties", Dispatcher.Query(new GetPropertiesQuery() { Type = Type, Language = Language.Csharp, IsCommand = meta.IsCommand }) },
                                     { "Response", Dispatcher.Query(new GetPropertiesQuery() { Type = responseOfType, Language = Language.Csharp, IsCommand = meta.IsCommand }) },
                                     { "InnerResponses", Dispatcher.Query(new GetInnerResponseTypesQuery() { Type = Type }) },
                                     { "IsNotifyPropertyChanged", meta.IsNotifyPropertyChanged },
                             };
            engine.Initialize();
            return engine.TransformText();
        }
    }
}