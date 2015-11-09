namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;

    #endregion

    public class AndroidTaskCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });
            var names = Dispatcher.Query(new GetNameFromTypeQuery(Type));
            var task = new Android_Task
                       {
                               Session = new Dictionary<string, object>
                                         {
                                                 { "Package", meta.Package }, 
                                                 { "Namespace", meta.Namespace }, 
                                                 { "Listener", names[GetNameFromTypeQuery.ModeOf.Listener] }, 
                                                 { "Request", names[GetNameFromTypeQuery.ModeOf.Request] }, 
                                                 { "Response", names[GetNameFromTypeQuery.ModeOf.Response] }, 
                                                 { "Name", names[GetNameFromTypeQuery.ModeOf.Task] }, 
                                                 { "HasRequest", Dispatcher.Query(new GetPropertiesFromTypeQuery { Type = Type, Device = DeviceOfType.Android, IsCommand = meta.IsCommand }).Any() }, 
                                                 { "Type", meta.IsCommand ? "Push" : "Query" }, 
                                         }
                       };
            task.Initialize();
            return task.TransformText();
        }
    }
}