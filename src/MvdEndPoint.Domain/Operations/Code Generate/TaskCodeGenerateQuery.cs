namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

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
            var propertiesByRequest = Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type });            
            task.Session = new Dictionary<string, object>
                               {
                                       { "Listener", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) },
                                       { "Request", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Request, Type = Type }) },
                                       { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) },
                                       { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Task, Type = Type }) },
                                       { "Url", Dispatcher.Query(new GetUrlByTypeQuery { Type = Type, BaseUrl = BaseUrl }) },
                                       { "PropertiesByResponse", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = responseType }) },
                                       { "PropertiesByRequest", propertiesByRequest },
                                       { "IsGet", isGet },
                                       { "HasRequest", propertiesByRequest.Any() },
                               };

            task.Initialize();
            return task.TransformText();
        }
    }
}