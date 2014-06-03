namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(ConvertCSharpTypeToJavaQuery))]
    public class When_convert_csharp_type_to_java
    {
        #region Fake classes

        class FakeType { }

        #endregion

        #region Establish value

        static void Compare(Type csharp, string java)
        {
            var query = Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, csharp));
            query.Execute();
            query.Result.ShouldEqual(java);
        }

        #endregion

        It should_be_custom = () => Compare(typeof(FakeType), "FakeType");

        It should_be_string = () => Compare(typeof(string), ConvertCSharpTypeToJavaQuery.String);

        It should_be_bool = () => Compare(typeof(bool), ConvertCSharpTypeToJavaQuery.Boolean);

        It should_be_bool_nullable = () => Compare(typeof(bool?), ConvertCSharpTypeToJavaQuery.BooleanAsNullable);

        It should_be_byte = () => Compare(typeof(byte), ConvertCSharpTypeToJavaQuery.Byte);

        It should_be_sbyte = () => Compare(typeof(sbyte), ConvertCSharpTypeToJavaQuery.Byte);

        It should_be_byte_as_nullable = () => Compare(typeof(byte?), ConvertCSharpTypeToJavaQuery.ByteAsNullable);

        It should_be_sbyte_as_nullable = () => Compare(typeof(sbyte?), ConvertCSharpTypeToJavaQuery.ByteAsNullable);

        It should_be_short_as_nullable = () => Compare(typeof(short?), ConvertCSharpTypeToJavaQuery.IntAsNull);

        It should_be_ushort_as_nullable = () => Compare(typeof(ushort?), ConvertCSharpTypeToJavaQuery.IntAsNull);

        It should_be_int = () => Compare(typeof(int), ConvertCSharpTypeToJavaQuery.Int);

        It should_be_uint = () => Compare(typeof(uint), ConvertCSharpTypeToJavaQuery.Int);

        It should_be_int_32 = () => Compare(typeof(Int32), ConvertCSharpTypeToJavaQuery.Int);

        It should_be_int_16 = () => Compare(typeof(Int16), ConvertCSharpTypeToJavaQuery.Int);

        It should_be_int_64 = () => Compare(typeof(Int64), ConvertCSharpTypeToJavaQuery.Long);

        It should_be_short = () => Compare(typeof(short), ConvertCSharpTypeToJavaQuery.Int);

        It should_be_ushort = () => Compare(typeof(ushort), ConvertCSharpTypeToJavaQuery.Int);

        It should_be_int_as_null = () => Compare(typeof(int?), ConvertCSharpTypeToJavaQuery.IntAsNull);

        It should_be_int_32_as_null = () => Compare(typeof(Int32?), ConvertCSharpTypeToJavaQuery.IntAsNull);

        It should_be_int_16_as_null = () => Compare(typeof(Int16?), ConvertCSharpTypeToJavaQuery.IntAsNull);

        It should_be_int_64_as_null = () => Compare(typeof(Int64?), ConvertCSharpTypeToJavaQuery.LongAsNullable);

        It should_be_uint_as_null = () => Compare(typeof(uint?), ConvertCSharpTypeToJavaQuery.IntAsNull);

        It should_be_long = () => Compare(typeof(long), ConvertCSharpTypeToJavaQuery.Long);

        It should_be_ulong = () => Compare(typeof(ulong), ConvertCSharpTypeToJavaQuery.Long);

        It should_be_long_as_nullable = () => Compare(typeof(long?), ConvertCSharpTypeToJavaQuery.LongAsNullable);

        It should_be_ulong_as_nullable = () => Compare(typeof(ulong?), ConvertCSharpTypeToJavaQuery.LongAsNullable);

        It should_be_float = () => Compare(typeof(float), ConvertCSharpTypeToJavaQuery.Float);

        It should_be_float_as_nullable = () => Compare(typeof(float?), ConvertCSharpTypeToJavaQuery.FloatAsNull);

        It should_be_double = () => Compare(typeof(double), ConvertCSharpTypeToJavaQuery.Double);

        It should_be_double_as_nullable = () => Compare(typeof(double?), ConvertCSharpTypeToJavaQuery.DoubleAsNullable);

        It should_be_decimal = () => Compare(typeof(decimal), ConvertCSharpTypeToJavaQuery.Decimal);

        It should_be_decimal_as_nullable = () => Compare(typeof(decimal?), ConvertCSharpTypeToJavaQuery.Decimal);

        It should_be_char = () => Compare(typeof(char), ConvertCSharpTypeToJavaQuery.Char);

        It should_be_char_as_nullable = () => Compare(typeof(char?), ConvertCSharpTypeToJavaQuery.CharAsNullable);

        It should_be_date_time = () => Compare(typeof(DateTime), ConvertCSharpTypeToJavaQuery.Date);

        It should_be_date_time_as_nullable = () => Compare(typeof(DateTime?), ConvertCSharpTypeToJavaQuery.Date);
    }
}