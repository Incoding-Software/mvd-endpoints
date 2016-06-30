namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Http;
    using Incoding.Extensions;

    #endregion

    public class HttpSampleCodeGenerateQuery : QueryBase<string>
    {
        public object Instance { get; set; }

        protected override string ExecuteResult()
        {
            var type = Instance.GetType();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery() { Type = type });
            var uri = Dispatcher.Query(new GetUriByTypeQuery() { Type = type });
            var tmpl = new Http_Sample();
            tmpl.Session = new Dictionary<string, object>()
                           {
                                   { "Verb", uri.Verb },
                                   { "Host", uri.Authority },
                                   { "Url", uri.Url },
                                   { "Body", Dispatcher.Query(new GetPropertiesQuery() { Type = type, IsCommand = meta.IsCommand }).Select(r => "{0}=@{1}".F(r.Name, r.Type)).AsString("&") }
                           };
            tmpl.Initialize();
            return tmpl.TransformText();
        }
    }
}