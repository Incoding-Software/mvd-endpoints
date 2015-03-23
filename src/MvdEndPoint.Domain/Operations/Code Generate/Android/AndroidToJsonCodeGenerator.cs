namespace MvdEndPoint.Domain.Operations
{
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate.Android;

    public class AndroidToJsonCodeGenerator:QueryBase<string>
    {
        protected override string ExecuteResult()
        {
            Android_ToJson tmpl = new Android_ToJson();
            tmpl.Session = new Dictionary<string, object>()
                           {
                                   
                           };
            tmpl.Session.Set("MappingJsonMethodByType", new Dictionary<string, string>
                                          {
                                                  { ConvertCSharpTypeToJavaQuery.String, "getString" },
                                                  { ConvertCSharpTypeToJavaQuery.Int, "getInt" },
                                                  { "int64", "getInt" },
                                                  { ConvertCSharpTypeToJavaQuery.Double, "getDouble" },
                                                  { ConvertCSharpTypeToJavaQuery.Boolean, "getBoolean" },
                                                  { typeof(long).Name, "getLong" },
                                          });
            return tmpl.TransformText();
        }
    }
}