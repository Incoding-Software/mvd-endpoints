namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class TaskCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var task = new Android_Task();

            task.Session = new Dictionary<string, object>
                               {
                                       { "Listener", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) },
                                       { "Request", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Request, Type = Type }) },
                                       { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) },
                                       { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Task, Type = Type }) },
                                       { "HasRequest", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type }).Any() },
                                       { "IsGet", !Type.IsImplement<CommandBase>() },
                               };

            task.Initialize();
            return task.TransformText();
        }
    }
}