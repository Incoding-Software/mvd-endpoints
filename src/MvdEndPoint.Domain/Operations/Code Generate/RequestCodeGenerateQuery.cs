namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class RequestCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var dto = new Android_Request();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });
            dto.Session = new Dictionary<string, object>
                              {
                                      { "Namespace", meta.Namespace },
                                      { "Package", meta.Package },
                                      { "Type", meta.Name },
                                      {
                                              "Name", Dispatcher.Query(new GetNameFromTypeQuery
                                                                           {
                                                                                   Type = Type,
                                                                                   Mode = GetNameFromTypeQuery.ModeOf.Request
                                                                           })
                                      },
                                      { "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type }) },
                                      { "IsGet", !Type.IsImplement<CommandBase>() },
                                      { "Url", Dispatcher.Query(new GetUrlByTypeQuery { Type = Type, BaseUrl = BaseUrl }) },
                              };
            dto.Initialize();
            return dto.TransformText();
        }
    }
}