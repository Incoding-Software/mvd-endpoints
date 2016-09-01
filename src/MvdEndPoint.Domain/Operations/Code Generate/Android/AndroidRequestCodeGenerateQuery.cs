namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Android;

    #endregion

    public class AndroidRequestCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });
            var names = Dispatcher.Query(new GetNameFromTypeQuery(Type));
            var task = new Android_Request()
                       {
                               Session = new Dictionary<string, object>
                                         {
                                                 { "Namespace", meta.Namespace },
                                                 { "Listener", names[GetNameFromTypeQuery.ModeOf.Listener] },
                                                 { "Response", names[GetNameFromTypeQuery.ModeOf.Response] },
                                                 { "Name", names[GetNameFromTypeQuery.ModeOf.Request] },
                                                 { "Type", meta.IsCommand ? "Push" : "Query" },
                                                 { "IncType", meta.Name },
                                                 { "Properties", Dispatcher.Query(new GetPropertiesQuery { Type = Type, Language = Language.JavaCE, IsCommand = meta.IsCommand }) },
                                                 { "MappingJsonMethodByType", AndroidNestedClassCodeGenerateQuery.MappingJsonMethodByType }
                                         }
                       };
            task.Initialize();
            return task.TransformText();
        }
    }
}