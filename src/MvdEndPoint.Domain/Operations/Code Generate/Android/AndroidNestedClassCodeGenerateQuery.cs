namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Android;

    #endregion

    public class AndroidNestedClassCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public string Namespace { get; set; }

        #endregion

        public static Dictionary<string, string> MappingJsonMethodByType = new Dictionary<string, string>
                                                                           {
                                                                                   { ConvertCSharpTypeToTargetQuery.ToJavaQuery.String, "getString" },
                                                                                   { ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int, "getInt" },
                                                                                   { "int64", "getInt" },
                                                                                   { ConvertCSharpTypeToTargetQuery.ToJavaQuery.Double, "getDouble" },
                                                                                   { ConvertCSharpTypeToTargetQuery.ToJavaQuery.Boolean, "getBoolean" },
                                                                                   { typeof(long).Name, "getLong" },
                                                                           };

        protected override string ExecuteResult()
        {
            var tmpl = new Android_Nested();
            tmpl.Session = new Dictionary<string, object>()
                           {
                                   { "Namespace", Namespace },
                                   { "Name", Dispatcher.Query(new GetNameFromTypeQuery(Type))[GetNameFromTypeQuery.ModeOf.Nested] },
                                   { "Properties", Dispatcher.Query(new GetPropertiesQuery() { Type = Type, Device = DeviceOfType.Android, IsCommand = false }) },
                                   { "MappingJsonMethodByType", MappingJsonMethodByType }
                           };
            tmpl.Initialize();
            return tmpl.TransformText();
        }
    }
}