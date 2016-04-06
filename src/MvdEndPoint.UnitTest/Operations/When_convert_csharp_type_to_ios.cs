namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(ConvertCSharpTypeToTargetQuery.ToIosQuery))]
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
            var query = Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery.ToIosQuery>(dsl => dsl.Tuning(r => r.Type, csharp));
            var mock = MockQuery<ConvertCSharpTypeToTargetQuery.ToIosQuery, string>.When(query);
            mock.Execute();
            mock.ShouldBeIsResult(ios);
        }

        #endregion

        It should_be_custom = () => Compare(typeof(FakeType), "FakeType");

        It should_be_string_as_array = () => Compare(typeof(string[]), ConvertCSharpTypeToTargetQuery.ToIosQuery.Array);

        It should_be_string = () => Compare(typeof(string), ConvertCSharpTypeToTargetQuery.ToIosQuery.String);

        It should_be_object_as_string = () => Compare(typeof(object), ConvertCSharpTypeToTargetQuery.ToIosQuery.String);

        It should_be_bool = () => Compare(typeof(bool), ConvertCSharpTypeToTargetQuery.ToIosQuery.Boolean);

        It should_be_bool_nullable = () => Compare(typeof(bool?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Boolean);

        It should_be_short_as_nullable = () => Compare(typeof(short?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_ushort_as_nullable = () => Compare(typeof(ushort?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int = () => Compare(typeof(int), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_uint = () => Compare(typeof(uint), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int_32 = () => Compare(typeof(Int32), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int_16 = () => Compare(typeof(Int16), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int_64 = () => Compare(typeof(Int64), ConvertCSharpTypeToTargetQuery.ToIosQuery.Long);

        It should_be_short = () => Compare(typeof(short), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_ushort = () => Compare(typeof(ushort), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int_as_null = () => Compare(typeof(int?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int_32_as_null = () => Compare(typeof(Int32?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int_16_as_null = () => Compare(typeof(Int16?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_int_64_as_null = () => Compare(typeof(Int64?), ConvertCSharpTypeToTargetQuery.ToIosQuery.LongAsNullable);

        It should_be_uint_as_null = () => Compare(typeof(uint?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Int);

        It should_be_long = () => Compare(typeof(long), ConvertCSharpTypeToTargetQuery.ToIosQuery.Long);

        It should_be_ulong = () => Compare(typeof(ulong), ConvertCSharpTypeToTargetQuery.ToIosQuery.Long);

        It should_be_long_as_nullable = () => Compare(typeof(long?), ConvertCSharpTypeToTargetQuery.ToIosQuery.LongAsNullable);

        It should_be_ulong_as_nullable = () => Compare(typeof(ulong?), ConvertCSharpTypeToTargetQuery.ToIosQuery.LongAsNullable);

        It should_be_float = () => Compare(typeof(float), ConvertCSharpTypeToTargetQuery.ToIosQuery.Float);

        It should_be_float_as_nullable = () => Compare(typeof(float?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Float);

        It should_be_double = () => Compare(typeof(double), ConvertCSharpTypeToTargetQuery.ToIosQuery.Double);

        It should_be_double_as_nullable = () => Compare(typeof(double?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Double);

        It should_be_date_time = () => Compare(typeof(DateTime), ConvertCSharpTypeToTargetQuery.ToIosQuery.Date);

        It should_be_date_time_as_nullable = () => Compare(typeof(DateTime?), ConvertCSharpTypeToTargetQuery.ToIosQuery.Date);

        It should_be_enum = () =>
                            {
                                string result = Pleasure.Generator.String();
                                var mockQuery = MockQuery<ConvertCSharpTypeToTargetQuery.ToIosQuery, string>
                                        .When(Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery.ToIosQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum))))
                                        .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum))), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                      {
                                                                                                                                                              { GetNameFromTypeQuery.ModeOf.Enum, result }, 
                                                                                                                                                      });
                                mockQuery.Execute();
                                mockQuery.ShouldBeIsResult(result);
                            };
    }
}