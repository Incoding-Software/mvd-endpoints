namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class ConvertCSharpTypeToIosQuery : QueryBase<string>
    {
        #region Constants

        public const string Boolean = "BOOL";

        public const string String = "NSString";

        public const string Array = "NSMutableArray";

        public const string Data = "NSData";

        public const string Double = "double";

        public const string Long = "long";

        public const string LongAsNullable = "java.lang.Long";

        public const string Int = "NSInteger";

        public const string Float = "float";

        public const string Date = "NSDate";

        #endregion

        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            if (Type.IsEnum)
                return Dispatcher.Query(new GetNameFromTypeQuery(Type))[GetNameFromTypeQuery.ModeOf.Enum];

            var primitive = new List<Tuple<Type[], string>>
                            {
                                    new Tuple<Type[], string>(new[] { typeof(HttpPostedFileBase), typeof(byte[]) }, Data), 
                                    new Tuple<Type[], string>(new[] { typeof(object), typeof(string) }, String), 
                                    new Tuple<Type[], string>(new[] { typeof(bool), typeof(bool?) }, Boolean), 
                                    new Tuple<Type[], string>(new[] { typeof(float), typeof(float?) }, Float), 
                                    new Tuple<Type[], string>(new[] { typeof(long), typeof(ulong) }, Long), 
                                    new Tuple<Type[], string>(new[] { typeof(long?), typeof(ulong?) }, LongAsNullable), 
                                    new Tuple<Type[], string>(new[] { typeof(double), typeof(double?) }, Double), 
                                    new Tuple<Type[], string>(new[]
                                                              {
                                                                      typeof(int), typeof(Int32), typeof(short), typeof(ushort), typeof(uint), 
                                                                      typeof(int?), typeof(Int32?), typeof(short?), typeof(ushort?), typeof(uint?)
                                                              }, Int), 
                                    new Tuple<Type[], string>(new[] { typeof(DateTime), typeof(DateTime?) }, Date), 
                            }
                    .SingleOrDefault(r => r.Item1.Contains(Type));

            return primitive.With(r => r.Item2).Recovery(() => Type.IsImplement<IEnumerable>() ? Array : Type.Name);
        }
    }
}