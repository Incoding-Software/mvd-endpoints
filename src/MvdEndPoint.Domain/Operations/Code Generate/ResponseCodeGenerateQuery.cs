namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class ResponseCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var dto = new Android_Resonse();
            dto.Session = new Dictionary<string, object>
                              {
                                      {
                                              "Name", Dispatcher.Query(new GetNameFromTypeQuery
                                                                           {
                                                                                   Type = Type,
                                                                                   Mode = GetNameFromTypeQuery.ModeOf.Response
                                                                           })
                                      },
                                      { "IsCommand", Type.IsImplement<CommandBase>() }
                              };

            if (!Type.IsImplement<CommandBase>())
            {
                dto.Session.Add("MappingJsonMethodByType", new Dictionary<string, string>
                                                               {
                                                                       { ConvertCSharpTypeToJavaQuery.String, "getString" },
                                                                       { ConvertCSharpTypeToJavaQuery.Int, "getInt" },
                                                                       { ConvertCSharpTypeToJavaQuery.Double, "getDouble" },
                                                                       { typeof(long).Name, "getLong" },
                                                               });
                dto.Session.Add("Properties", Dispatcher.Query(new GetPropertiesByTypeQuery
                                                                   {
                                                                           Type = Type.BaseType.GenericTypeArguments[0]
                                                                   }));
            }
            dto.Initialize();
            return dto.TransformText();
        }
    }
}