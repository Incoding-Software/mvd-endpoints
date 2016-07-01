namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Http;
    using Incoding.Extensions;

    #endregion

    public class CurlSampleCodeGenerateQuery : QueryBase<string>
    {
        public object Instance { get; set; }

        protected override string ExecuteResult()
        {
            var type = Instance.GetType();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery() { Type = type });
            var uri = Dispatcher.Query(new GetUriByTypeQuery() { Type = type });
            var tmpl = new Curl_Sample();
            tmpl.Session = new Dictionary<string, object>()
                           {
                                   { "Verb", uri.Verb },
                                   { "Host", uri.Authority },
                                   { "Url", uri.Url },
                                   { "Body", Instance.ToJsonString() }
                           };
            tmpl.Initialize();
            return tmpl.TransformText();
        }
    }
}