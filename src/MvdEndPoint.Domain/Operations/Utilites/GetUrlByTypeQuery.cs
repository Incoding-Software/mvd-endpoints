namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
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
            return "{0}/Dispatcher/".F(BaseUrl).AppendSegment(isCommand ? "Push" : "Query");
        }
    }
}