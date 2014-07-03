namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class RequestCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public string BaseUrl { get; set; }

        public string Namespace { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var dto = new Android_Request();
            dto.Session = new Dictionary<string, object>
                              {
                                      { "Namespace", Namespace }, 
                                      {
                                              "Name", Dispatcher.Query(new GetNameFromTypeQuery
                                                                           {
                                                                                   Type = Type, 
                                                                                   Mode = GetNameFromTypeQuery.ModeOf.Request
                                                                           })
                                      }, 
                                      {
                                              "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type })
                                                                      .ToDictionary(r => r.Name, r => r.Type)
                                      }, 
                                      { "IsGet", !Type.IsImplement<CommandBase>() }, 
                                      { "Url", Dispatcher.Query(new GetUrlByTypeQuery { Type = Type, BaseUrl = BaseUrl }) }, 
                              };
            dto.Initialize();
            return dto.TransformText();
        }
    }
}