namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations;
    using MvdEndPoint.Domain.Operations.Code_Generate.Android;

    #endregion

    public class AndroidResponseCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        const string propertiesAsKey = "Properties";

        const string nestedAsKey = "Nested";

        const string mappingAsKey = "MappingJsonMethodByType";

        protected override string ExecuteResult()
        {
            bool isQuery = !Type.IsImplement<CommandBase>();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });

            var session = new Dictionary<string, object>
                          {
                                  { "Namespace", meta.Namespace },
                                  { "Package", meta.Package },
                                  { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Response }) },
                                  { nestedAsKey, new List<string>() },
                                  { mappingAsKey, new Dictionary<string, string>() },
                                  { propertiesAsKey, new Dictionary<string, string>() },
                                  { "IsQuery", isQuery },
                          };

            if (isQuery)
            {
                var responseType = Type.BaseType.GenericTypeArguments[0];
                var properties = Dispatcher.Query(new GetPropertiesFromTypeQuery
                                                  {
                                                          Type = responseType,
                                                          Device = DeviceOfType.Android,
                                                          IsCommand = false
                                                  });

                session.Add("IsArray", responseType.IsImplement(typeof(IEnumerable<>)));
                session.Set(nestedAsKey, properties.Where(r => r.Attributes.HasFlag(GetPropertiesFromTypeQuery.Response.OfAttributes.IsClass))
                                                   .Select(r => Dispatcher.Query(new AndroidNestedClassCodeGenerateQuery()
                                                                                 {
                                                                                         Type = r.Target,
                                                                                         Namespace = meta.Namespace,                                                                                         
                                                                                 }))
                                                   .ToList());

                session.Set(propertiesAsKey, properties);
                session.Set(mappingAsKey, new Dictionary<string, string>
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