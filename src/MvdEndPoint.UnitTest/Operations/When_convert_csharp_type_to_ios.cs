namespace MvdEndPoint.UnitTest
{
    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    [Subject(typeof(ConvertCSharpTypeToIosQuery))]
    public class When_convert_csharp_type_to_ios
    {
        #region Fake classes

        class FakeType { }

        #endregion

        #region Establish value

        enum FakeEnum
        { }

        static void Compare(Type csharp, string ios)
        {
            var query = Pleasure.Generator.Invent<ConvertCSharpTypeToIosQuery>(dsl => dsl.Tuning(r => r.Type, csharp));
            query.Execute();
            query.Result.ShouldEqual(ios);
        }

        #endregion

        It should_be_custom = () => Compare(typeof(FakeType), "FakeType");

        It should_be_string_as_array = () => Compare(typeof(string[]), ConvertCSharpTypeToIosQuery.Array);

        It should_be_string = () => Compare(typeof(string), ConvertCSharpTypeToIosQuery.String);

        It should_be_object_as_string = () => Compare(typeof(object), ConvertCSharpTypeToIosQuery.String);

        It should_be_bool = () => Compare(typeof(bool), ConvertCSharpTypeToIosQuery.Boolean);

        It should_be_bool_nullable = () => Compare(typeof(bool?), ConvertCSharpTypeToIosQuery.BooleanAsNullable);

        It should_be_byte = () => Compare(typeof(byte), ConvertCSharpTypeToIosQuery.Byte);

        It should_be_sbyte = () => Compare(typeof(sbyte), ConvertCSharpTypeToIosQuery.Byte);

        It should_be_byte_as_nullable = () => Compare(typeof(byte?), ConvertCSharpTypeToIosQuery.ByteAsNullable);

        It should_be_sbyte_as_nullable = () => Compare(typeof(sbyte?), ConvertCSharpTypeToIosQuery.ByteAsNullable);

        It should_be_short_as_nullable = () => Compare(typeof(short?), ConvertCSharpTypeToIosQuery.IntAsNull);

        It should_be_ushort_as_nullable = () => Compare(typeof(ushort?), ConvertCSharpTypeToIosQuery.IntAsNull);

        It should_be_int = () => Compare(typeof(int), ConvertCSharpTypeToIosQuery.Int);

        It should_be_uint = () => Compare(typeof(uint), ConvertCSharpTypeToIosQuery.Int);

        It should_be_int_32 = () => Compare(typeof(Int32), ConvertCSharpTypeToIosQuery.Int);

        It should_be_int_16 = () => Compare(typeof(Int16), ConvertCSharpTypeToIosQuery.Int);

        It should_be_int_64 = () => Compare(typeof(Int64), ConvertCSharpTypeToIosQuery.Long);

        It should_be_short = () => Compare(typeof(short), ConvertCSharpTypeToIosQuery.Int);

        It should_be_ushort = () => Compare(typeof(ushort), ConvertCSharpTypeToIosQuery.Int);

        It should_be_int_as_null = () => Compare(typeof(int?), ConvertCSharpTypeToIosQuery.IntAsNull);

        It should_be_int_32_as_null = () => Compare(typeof(Int32?), ConvertCSharpTypeToIosQuery.IntAsNull);

        It should_be_int_16_as_null = () => Compare(typeof(Int16?), ConvertCSharpTypeToIosQuery.IntAsNull);

        It should_be_int_64_as_null = () => Compare(typeof(Int64?), ConvertCSharpTypeToIosQuery.LongAsNullable);

        It should_be_uint_as_null = () => Compare(typeof(uint?), ConvertCSharpTypeToIosQuery.IntAsNull);

        It should_be_long = () => Compare(typeof(long), ConvertCSharpTypeToIosQuery.Long);

        It should_be_ulong = () => Compare(typeof(ulong), ConvertCSharpTypeToIosQuery.Long);

        It should_be_long_as_nullable = () => Compare(typeof(long?), ConvertCSharpTypeToIosQuery.LongAsNullable);

        It should_be_ulong_as_nullable = () => Compare(typeof(ulong?), ConvertCSharpTypeToIosQuery.LongAsNullable);

        It should_be_float = () => Compare(typeof(float), ConvertCSharpTypeToIosQuery.Float);

        It should_be_float_as_nullable = () => Compare(typeof(float?), ConvertCSharpTypeToIosQuery.FloatAsNull);

        It should_be_double = () => Compare(typeof(double), ConvertCSharpTypeToIosQuery.Double);

        It should_be_double_as_nullable = () => Compare(typeof(double?), ConvertCSharpTypeToIosQuery.DoubleAsNullable);

        It should_be_decimal = () => Compare(typeof(decimal), ConvertCSharpTypeToIosQuery.Decimal);

        It should_be_decimal_as_nullable = () => Compare(typeof(decimal?), ConvertCSharpTypeToIosQuery.Decimal);

        It should_be_char = () => Compare(typeof(char), ConvertCSharpTypeToIosQuery.Char);

        It should_be_char_as_nullable = () => Compare(typeof(char?), ConvertCSharpTypeToIosQuery.CharAsNullable);

        It should_be_date_time = () => Compare(typeof(DateTime), ConvertCSharpTypeToIosQuery.Date);

        It should_be_date_time_as_nullable = () => Compare(typeof(DateTime?), ConvertCSharpTypeToIosQuery.Date);

        It should_be_enum = () =>
                                {
                                    string result = Pleasure.Generator.String();
                                    var mockQuery = MockQuery<ConvertCSharpTypeToIosQuery, string>
                                            .When(Pleasure.Generator.Invent<ConvertCSharpTypeToIosQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum))))
                                            .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum))
                                                                                                                 .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Enum)), result);
                                    mockQuery.Original.Execute();
                                    mockQuery.ShouldBeIsResult(result);
                                };
    }
}