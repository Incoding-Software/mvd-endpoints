namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations;

    #endregion

    public class TaskCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var task = new Android_Task();

            var responseType = Type.BaseType.GetGenericArguments()[0];
            task.Session = new Dictionary<string, object>
                               {
                                       { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Task, Type = Type }) },
                                       { "Listener", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) },
                                       { "Request", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Request, Type = Type }) },
                                       { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) },
                                       { "PropertiesByResponse", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = responseType }) },
                                       { "PropertiesByRequest", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type }) },
                               };

            task.Initialize();
            return task.TransformText();
        }
    }
}