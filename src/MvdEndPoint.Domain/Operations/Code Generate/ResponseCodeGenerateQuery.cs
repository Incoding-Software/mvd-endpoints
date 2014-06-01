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
            var dto = new Android_Response();
            var responseType = Type.BaseType.GenericTypeArguments[0];
            dto.Session = new Dictionary<string, object>
                              {
                                      { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Response }) }, 
                                      { "IsArray", responseType.IsImplement(typeof(IEnumerable<>)) }, 
                                      {
                                              "MappingJsonMethodByType", new Dictionary<string, string>
                                                                             {
                                                                                     { ConvertCSharpTypeToJavaQuery.String, "getString" }, 
                                                                                     { ConvertCSharpTypeToJavaQuery.Int, "getInt" }, 
                                                                                     { ConvertCSharpTypeToJavaQuery.Double, "getDouble" }, 
                                                                                     { typeof(long).Name, "getLong" }, 
                                                                             }
                                      }, 
                                      {
                                              "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery
                                                                                 {
                                                                                         Type = responseType
                                                                                 })
                                      }
                              };
            dto.Initialize();
            return dto.TransformText();
        }
    }
}