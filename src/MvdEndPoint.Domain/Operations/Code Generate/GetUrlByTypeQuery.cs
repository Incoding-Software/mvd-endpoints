namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetUrlByTypeQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            bool isCommand = Type.IsImplement<CommandBase>();
            string url = "{0}/Dispatcher/".F(BaseUrl)
                                          .AppendSegment(isCommand ? "Push" : "Query")
                                          .AppendToQueryString(new { incType = Type.Name });
            if (isCommand)
                return url;

            return Dispatcher.Query(new GetPropertiesByTypeQuery
                                        {
                                                Type = Type,
                                        })
                             .Aggregate(url, (current, property) => current + "&{0}=%s".F(property.Key));
        }
    }
}