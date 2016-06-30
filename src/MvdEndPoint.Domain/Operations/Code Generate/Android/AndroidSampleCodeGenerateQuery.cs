namespace Incoding.Endpoint
{
    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Android;

    public class AndroidSampleCodeGenerateQuery:QueryBase<string>
    {
        public object Instance { get; set; }

        protected override string ExecuteResult()
        {
            var type = Instance.GetType();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = type });
            var names = Dispatcher.Query(new GetNameFromTypeQuery(type));
            var tmpl = new Android_Sample()
                       {
                               Session = new Dictionary<string, object>
                                         {
                                                 { "Type", meta.Name },
                                                 { "Namespace", meta.Namespace },
                                                 { "Listener", names[GetNameFromTypeQuery.ModeOf.Listener] },
                                                 { "Response", names[GetNameFromTypeQuery.ModeOf.Response] },
                                                 { "ResponseAsArray", meta.ResponseAsArray },
                                                 { "Request", names[GetNameFromTypeQuery.ModeOf.Request] },
                                                 { "Properties", Dispatcher.Query(new GetPropertiesQuery { Type = type, Device = DeviceOfType.Android, IsCommand = meta.IsCommand }) },
                                         }
                       };
            tmpl.Initialize();
            return tmpl.TransformText();

        }
    }
}