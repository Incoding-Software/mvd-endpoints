namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Maybe;

    #endregion

    public class ConvertCSharpTypeToJavaQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var primitive = new List<Tuple<Type[], string>>
                                {
                                        new Tuple<Type[], string>(new[] { typeof(bool) }, "Boolean"),
                                        new Tuple<Type[], string>(new[] { typeof(bool?) }, "java.lang.Boolean"),
                                        new Tuple<Type[], string>(new[] { typeof(byte), typeof(sbyte) }, "byte"),
                                        new Tuple<Type[], string>(new[] { typeof(int), typeof(Int16), typeof(Int32), typeof(Int64) }, "Integer"),
                                }
                    .SingleOrDefault(r => r.Item1.Contains(Type));

            return primitive.With(r => r.Item2).Recovery(Type.Name);
        }
    }
}