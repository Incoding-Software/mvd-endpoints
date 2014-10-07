namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    
    #endregion

    public class AndroidResponseCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }
        
        #endregion

        protected override string ExecuteResult()
        {
            bool isQuery = !Type.IsImplement<CommandBase>();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });
            var session = new Dictionary<string, object>
                              {
                                      { "Namespace", meta.Namespace },
                                      { "Package", meta.Package },
                                      { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Response }) },
                                      { "MappingJsonMethodByType", new Dictionary<string, string>() },
                                      { "Properties", new Dictionary<string, string>() },
                                      { "IsQuery", isQuery },
                              };

            if (isQuery)
            {
                var responseType = Type.BaseType.GenericTypeArguments[0];
                session.Add("IsArray", responseType.IsImplement(typeof(IEnumerable<>)));
                session.Set("Properties", Dispatcher.Query(new GetPropertiesByTypeQuery
                                                               {
                                                                       Type = responseType,
                                                                       Device = DeviceOfType.Android
                                                               }));
                session.Set("MappingJsonMethodByType", new Dictionary<string, string>
                                                           {
                                                                   { ConvertCSharpTypeToJavaQuery.String, "getString" },
                                                                   { ConvertCSharpTypeToJavaQuery.Int, "getInt" },
                                                                   { "int64", "getInt" },                                                                   
                                                                   { ConvertCSharpTypeToJavaQuery.Double, "getDouble" },
                                                                   { ConvertCSharpTypeToJavaQuery.Boolean, "getBoolean" },
                                                                   { typeof(long).Name, "getLong" },
                                                           });
            }
            var tmplAndroid = new Android_Response();
            tmplAndroid.Session = session;
            tmplAndroid.Initialize();
            return tmplAndroid.TransformText();
        }
    }
}