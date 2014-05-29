namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations;
    using MvdEndPoint.Domain.Operations.Code_Generate;
    using StructureMap.Pipeline;

    #endregion

    public class TaskCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var task = new Android_Task();

            var responseType = Type.BaseType.GetGenericArguments()[0];
            bool isGet = Type.BaseType.Name.Contains("QueryBase");
            task.Session = new Dictionary<string, object>
                               {
                                       { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Task, Type = Type }) },
                                       { "Listener", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) },
                                       { "Request", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Request, Type = Type }) },
                                       { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) },
                                       { "PropertiesByResponse", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = responseType }) },
                                       { "PropertiesByRequest", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type }) },
                                       { "Url", "{0}{1}".F(BaseUrl, Dispatcher.Query(new GetUrlByTypeQuery{Type = Type})) },
                                       { "IsGet", isGet }
                               };

            task.Initialize();
            return task.TransformText();
        }
    }
}