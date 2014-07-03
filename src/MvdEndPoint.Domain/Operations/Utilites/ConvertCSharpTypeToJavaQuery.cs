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
        #region Constants

        public const string Boolean = "Boolean";

        public const string String = "String";

        public const string Char = "char";

        public const string CharAsNullable = "java.lang.Character";

        public const string Decimal = "java.math.BigDecimal";

        public const string Double = "Double";

        public const string DoubleAsNullable = "java.lang.Double";

        public const string BooleanAsNullable = "java.lang.Boolean";

        public const string Byte = "byte";

        public const string ByteAsNullable = "java.lang.Byte";

        public const string Long = "long";

        public const string LongAsNullable = "java.lang.Long";

        public const string Int = "int";

        public const string IntAsNull = "java.lang.Integer";

        public const string Float = "float";

        public const string FloatAsNull = "java.lang.Float";

        public const string Date = "java.util.Date";

        #endregion

        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            if (Type.IsEnum)
            {
                return Dispatcher.Query(new GetNameFromTypeQuery
                                            {
                                                    Type = Type, 
                                                    Mode = GetNameFromTypeQuery.ModeOf.Enum
                                            });
            }

            var primitive = new List<Tuple<Type[], string>>
                                {
                                        new Tuple<Type[], string>(new[] { typeof(object) }, String), 
                                        new Tuple<Type[], string>(new[] { typeof(bool) }, Boolean), 
                                        new Tuple<Type[], string>(new[] { typeof(bool?) }, BooleanAsNullable), 
                                        new Tuple<Type[], string>(new[] { typeof(char) }, Char), 
                                        new Tuple<Type[], string>(new[] { typeof(char?) }, CharAsNullable), 
                                        new Tuple<Type[], string>(new[] { typeof(float) }, Float), 
                                        new Tuple<Type[], string>(new[] { typeof(float?) }, FloatAsNull), 
                                        new Tuple<Type[], string>(new[] { typeof(long), typeof(ulong) }, Long), 
                                        new Tuple<Type[], string>(new[] { typeof(long?), typeof(ulong?) }, LongAsNullable), 
                                        new Tuple<Type[], string>(new[] { typeof(double) }, Double), 
                                        new Tuple<Type[], string>(new[] { typeof(double?) }, DoubleAsNullable), 
                                        new Tuple<Type[], string>(new[] { typeof(decimal), typeof(decimal?) }, Decimal), 
                                        new Tuple<Type[], string>(new[] { typeof(byte), typeof(sbyte) }, Byte), 
                                        new Tuple<Type[], string>(new[] { typeof(byte?), typeof(sbyte?) }, ByteAsNullable), 
                                        new Tuple<Type[], string>(new[] { typeof(int), typeof(Int32), typeof(short), typeof(ushort), typeof(uint) }, Int), 
                                        new Tuple<Type[], string>(new[] { typeof(int?), typeof(Int32?), typeof(short?), typeof(ushort?), typeof(uint?) }, IntAsNull), 
                                        new Tuple<Type[], string>(new[] { typeof(DateTime), typeof(DateTime?)}, Date), 
                                }
                    .SingleOrDefault(r => r.Item1.Contains(Type));

            return primitive.With(r => r.Item2).Recovery(Type.Name);
        }
    }
}