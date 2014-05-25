namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations;
    using Incoding.Extensions;
    using System.Linq;

    #endregion

    public class ConvertCSharpTypeToJavaQuery : QueryBase<string>
    {
        public string Type { get; set; }

        protected override string ExecuteResult()
        {
            return new Dictionary<string, string>
                       {
                               { typeof(bool).Name, "Boolean" }
                       }.GetOrDefault(Type);
        }
    }

    public class RequestCodeGenerateQuery : QueryBase<string>
    {
        public string AssemblyQualifiedType { get; set; }

        protected override string ExecuteResult()
        {
            var type = Type.GetType(AssemblyQualifiedType);

            var response = new Android_Response();
            response.Session = new Dictionary<string, object>
                                   {
                                           { "Name", type.Name.Replace("Query", "Request") },
                                           {
                                                   "Properties", type.GetProperties()
                                                                     .Where(r => r.Name != "Result")
                                                                     .Select(r =>
                                                                                 {
                                                                                     string javaType = Dispatcher.Query(new ConvertCSharpTypeToJavaQuery
                                                                                                                            {
                                                                                                                                    Type = r.PropertyType.Name
                                                                                                                            });
                                                                                     return new KeyValuePair<string, string>(javaType, r.Name);
                                                                                 })
                                                                     .ToList()
                                           }
                                   };
            response.Initialize();
            return response.TransformText();
        }
    }
}