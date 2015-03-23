namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class AndroidTaskCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });
            var isCommand = Type.IsImplement<CommandBase>();
            var task = new Android_Task
                       {
                               Session = new Dictionary<string, object>
                                         {
                                                 { "Package", meta.Package },
                                                 { "Namespace", meta.Namespace },
                                                 { "Listener", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) },
                                                 { "Request", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Request, Type = Type }) },
                                                 { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) },
                                                 { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Task, Type = Type }) },
                                                 { "HasRequest", Dispatcher.Query(new GetPropertiesFromTypeQuery { Type = Type, Device = DeviceOfType.Android, IsCommand = isCommand }).Any() },
                                                 { "Type", isCommand ? "Push" : "Query" },
                                         }
                       };
            task.Initialize();
            return task.TransformText();
        }
    }
}