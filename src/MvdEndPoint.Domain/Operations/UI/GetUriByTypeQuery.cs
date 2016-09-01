namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class GetUriByTypeQuery : QueryBase<GetUriByTypeQuery.Response>
    {
        public Type Type { get; set; }

        protected override Response ExecuteResult()
        {
            bool isCommand = Dispatcher.Query(new IsCommandTypeQuery(Type));
            var methodInfo = typeof(UrlDispatcher).GetMethods().FirstOrDefault(r => r.Name == (isCommand ? "Push" : "Query"));
            var getUrl = methodInfo.MakeGenericMethod(Type).Invoke(new UrlHelper(HttpContext.Current.Request.RequestContext).Dispatcher(), new[] { Activator.CreateInstance(Type) });
            var scheme = HttpContext.Current.Request.Url.Scheme;
            var authority = HttpContext.Current.Request.Url.Authority;
            return new Response()
                   {
                           Verb = isCommand ? "POST" : "GET",
                           Url = isCommand
                                         ? getUrl.ToString()
                                         : getUrl.GetType().GetMethod("AsJson").Invoke(getUrl, new object[] { }).ToString(),
                           Scheme = scheme,
                           Authority = authority,
                           Host = "{0}://{1}".F(scheme, authority)
                   };
        }

        public class Response
        {
            public string Verb { get; set; }

            public string Url { get; set; }

            public string Scheme { get; set; }

            public string Authority { get; set; }

            public string Host { get; set; }
        }
    }
}