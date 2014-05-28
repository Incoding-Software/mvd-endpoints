namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class ConvertCSharpTypeToJavaQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            return new Dictionary<string, string>
                       {
                               { typeof(bool).Name, "Boolean" },
                               { typeof(bool?).Name, "java.lang.Boolean" },
                               { typeof(byte).Name, "byte" },
                               { typeof(sbyte).Name, "byte" },
                       }.GetOrDefault(Type.Name);
        }
    }
}