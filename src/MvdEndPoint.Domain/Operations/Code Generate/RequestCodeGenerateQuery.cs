namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class RequestCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var dto = new Android_Dto();
            dto.Session = new Dictionary<string, object>
                              {
                                      {
                                              "Name", Dispatcher.Query(new GetNameFromTypeQuery
                                                                           {
                                                                                   Type = Type,
                                                                                   Mode = GetNameFromTypeQuery.ModeOf.Request
                                                                           })
                                      },
                                      { "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type }) }
                              };
            dto.Initialize();
            return dto.TransformText();
        }
    }
}