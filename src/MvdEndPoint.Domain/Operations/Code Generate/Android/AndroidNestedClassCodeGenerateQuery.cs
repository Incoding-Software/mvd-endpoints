namespace MvdEndPoint.Domain.Operations
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate.Android;

    #endregion

    public class AndroidNestedClassCodeGenerateQuery : QueryBase<string>
    {
        public Type Type { get; set; }

        public string Namespace { get; set; }

        protected override string ExecuteResult()
        {
            return string.Empty;
            Android_Nested tmpl = new Android_Nested();
            tmpl.Session = new Dictionary<string, object>()
                           {
                                   { "Namespace", Namespace },
                                   { "Name", Dispatcher.Query(new GetNameFromTypeQuery() { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Nested }) },
                                   { "Properties", Dispatcher.Query(new GetPropertiesFromTypeQuery() { Type = Type, Device = DeviceOfType.Android, IsCommand = false }) },
                                   {
                                           "MappingJsonMethodByType", new Dictionary<string, string>
                                                                      {
                                                                              { ConvertCSharpTypeToJavaQuery.String, "getString" },
                                                                              { ConvertCSharpTypeToJavaQuery.Int, "getInt" },
                                                                              { "int64", "getInt" },
                                                                              { ConvertCSharpTypeToJavaQuery.Double, "getDouble" },
                                                                              { ConvertCSharpTypeToJavaQuery.Boolean, "getBoolean" },
                                                                              { typeof(long).Name, "getLong" },
                                                                      }
                                   }
                           };
            return tmpl.TransformText();
        }
    }
}