namespace MvdEndPoint.Domain
{
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;

    public class ConvertCSharpTypeToJavaQuery : QueryBase<string>
    {
        public string Type { get; set; }

        protected override string ExecuteResult()
        {
            return new Dictionary<string, string>
                       {
                               { typeof(bool).Name, "Boolean" }
                       }.GetOrDefault(this.Type);
        }
    }
}