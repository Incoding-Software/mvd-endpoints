namespace MvdEndPoint.Domain
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;

    public class GetUrlByTypeQuery : QueryBase<string>
    {
        public Type Type { get; set; }

        protected override string ExecuteResult()
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            bool isCommand = Type.IsImplement<CommandBase>();
            var methodInfo = typeof(UrlDispatcher).GetMethods().FirstOrDefault(r => r.Name == (isCommand ? "Push" : "Query"));
            var getUrl = methodInfo.MakeGenericMethod(Type).Invoke(url.Dispatcher(), new[] { Activator.CreateInstance(Type) });
            return isCommand
                           ? getUrl.ToString().Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0]
                           : getUrl.GetType().GetMethod("AsJson").Invoke(getUrl, new object[] { }).ToString();
        }
    }
}