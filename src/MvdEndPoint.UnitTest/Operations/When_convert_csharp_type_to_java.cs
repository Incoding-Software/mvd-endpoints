namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Web;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConvertCSharpTypeToTargetQuery.ToJavaQuery))]
    public class When_convert_csharp_type_to_java
    {
        #region Fake classes

        class FakeType { }

        #endregion

        #region Establish value

        enum FakeEnum
        { }

        static void Compare(Type csharp, string java)
        {
            var query = Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery.ToJavaQuery>(dsl => dsl.Tuning(r => r.Type, csharp));
            var mock = MockQuery<ConvertCSharpTypeToTargetQuery.ToJavaQuery, string>.When(query);
            mock.Execute();
            mock.ShouldBeIsResult(java);
        }

        #endregion

        It should_be_custom = () => Compare(typeof(FakeType), "FakeType");

        It should_be_string_as_array = () => Compare(typeof(string[]), ConvertCSharpTypeToTargetQuery.ToJavaQuery.StringAsArray);

        It should_be_byte_as_array = () => Compare(typeof(byte[]), ConvertCSharpTypeToTargetQuery.ToJavaQuery.ByteAsArray);

        It should_be_http_post_file_base = () => Compare(typeof(HttpPostedFileBase), ConvertCSharpTypeToTargetQuery.ToJavaQuery.ByteAsArray);

        It should_be_string = () => Compare(typeof(string), ConvertCSharpTypeToTargetQuery.ToJavaQuery.String);

        It should_be_object_as_string = () => Compare(typeof(object), ConvertCSharpTypeToTargetQuery.ToJavaQuery.String);

        It should_be_bool = () => Compare(typeof(bool), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Boolean);

        It should_be_bool_nullable = () => Compare(typeof(bool?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.BooleanAsNullable);

        It should_be_byte = () => Compare(typeof(byte), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Byte);

        It should_be_sbyte = () => Compare(typeof(sbyte), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Byte);

        It should_be_byte_as_nullable = () => Compare(typeof(byte?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.ByteAsNullable);

        It should_be_sbyte_as_nullable = () => Compare(typeof(sbyte?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.ByteAsNullable);

        It should_be_short_as_nullable = () => Compare(typeof(short?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.IntAsNull);

        It should_be_ushort_as_nullable = () => Compare(typeof(ushort?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.IntAsNull);

        It should_be_int = () => Compare(typeof(int), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int);

        It should_be_uint = () => Compare(typeof(uint), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int);

        It should_be_int_32 = () => Compare(typeof(Int32), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int);

        It should_be_int_16 = () => Compare(typeof(Int16), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int);

        It should_be_int_64 = () => Compare(typeof(Int64), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Long);

        It should_be_short = () => Compare(typeof(short), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int);

        It should_be_ushort = () => Compare(typeof(ushort), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int);

        It should_be_int_as_null = () => Compare(typeof(int?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.IntAsNull);

        It should_be_int_32_as_null = () => Compare(typeof(Int32?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.IntAsNull);

        It should_be_int_16_as_null = () => Compare(typeof(Int16?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.IntAsNull);

        It should_be_int_64_as_null = () => Compare(typeof(Int64?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.LongAsNullable);

        It should_be_uint_as_null = () => Compare(typeof(uint?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.IntAsNull);

        It should_be_long = () => Compare(typeof(long), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Long);

        It should_be_ulong = () => Compare(typeof(ulong), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Long);

        It should_be_long_as_nullable = () => Compare(typeof(long?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.LongAsNullable);

        It should_be_ulong_as_nullable = () => Compare(typeof(ulong?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.LongAsNullable);

        It should_be_float = () => Compare(typeof(float), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Float);

        It should_be_float_as_nullable = () => Compare(typeof(float?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.FloatAsNull);

        It should_be_double = () => Compare(typeof(double), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Double);

        It should_be_double_as_nullable = () => Compare(typeof(double?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.DoubleAsNullable);

        It should_be_decimal = () => Compare(typeof(decimal), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Decimal);

        It should_be_decimal_as_nullable = () => Compare(typeof(decimal?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Decimal);

        It should_be_char = () => Compare(typeof(char), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Char);

        It should_be_char_as_nullable = () => Compare(typeof(char?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.CharAsNullable);

        It should_be_date_time = () => Compare(typeof(DateTime), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Date);

        It should_be_date_time_as_nullable = () => Compare(typeof(DateTime?), ConvertCSharpTypeToTargetQuery.ToJavaQuery.Date);

        It should_be_enum = () =>
                            {
                                string result = Pleasure.Generator.String();
                                var mockQuery = MockQuery<ConvertCSharpTypeToTargetQuery.ToJavaQuery, string>
                                        .When(Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery.ToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum))))
                                        .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum))), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                      {
                                                                                                                                                              { GetNameFromTypeQuery.ModeOf.Enum, result }, 
                                                                                                                                                      });
                                mockQuery.Execute();
                                mockQuery.ShouldBeIsResult(result);
                            };
    }
}