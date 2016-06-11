namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class AndroidResponseCodeGenerateQuery : QueryBase<string>
    {
        #region Constants

        const string propertiesAsKey = "Properties";

        const string nestedAsKey = "Nested";

        const string mappingAsKey = "MappingJsonMethodByType";

        #endregion

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
                                  { "Name", Dispatcher.Query(new GetNameFromTypeQuery(Type))[GetNameFromTypeQuery.ModeOf.Response] }, 
                                  { nestedAsKey, new List<string>() }, 
                                  { mappingAsKey, new Dictionary<string, string>() }, 
                                  { propertiesAsKey, new Dictionary<string, string>() }, 
                                  { "IsQuery", isQuery }, 
                          };

            if (isQuery)
            {
                var responseType = Type.BaseType.GenericTypeArguments[0];
                var properties = Dispatcher.Query(new GetPropertiesQuery
                                                  {
                                                          Type = responseType, 
                                                          Device = DeviceOfType.Android, 
                                                          IsCommand = false
                                                  });

                session.Add("IsArray", responseType.IsImplement(typeof(IEnumerable<>)));
                session.Set(nestedAsKey, properties.Where(r => r.Attributes.HasFlag(GetPropertiesQuery.Response.OfAttributes.IsClass))
                                                   .Select(r => Dispatcher.Query(new AndroidNestedClassCodeGenerateQuery()
                                                                                 {
                                                                                         Type = r.Target, 
                                                                                         Namespace = meta.Namespace, 
                                                                                 }))
                                                   .ToList());

                session.Set(propertiesAsKey, properties);
                session.Set(mappingAsKey, new Dictionary<string, string>
                                          {
                                                  { ConvertCSharpTypeToTargetQuery.ToJavaQuery.String, "getString" }, 
                                                  { ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int, "getInt" }, 
                                                  { "int64", "getInt" }, 
                                                  { ConvertCSharpTypeToTargetQuery.ToJavaQuery.Double, "getDouble" }, 
                                                  { ConvertCSharpTypeToTargetQuery.ToJavaQuery.Boolean, "getBoolean" }, 
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