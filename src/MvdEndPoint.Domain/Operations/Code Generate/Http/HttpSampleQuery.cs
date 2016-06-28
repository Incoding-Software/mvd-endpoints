namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Http;

    #endregion

    public class HttpSampleQuery : QueryBase<string>
    {
        public object Instance { get; set; }

        protected override string ExecuteResult()
        {
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery() { Type = Instance.GetType() });
            var tmpl = new Http_Sample();
            tmpl.Session = new Dictionary<string, object>()
                           {
                                   { "Verb", meta.IsCommand ? "POST" : "GET" },
                                   { "Host", "" },
                                   { "Url", "" }
                           };
            tmpl.Initialize();
            return tmpl.TransformText();
        }
    }
}