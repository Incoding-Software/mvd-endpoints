namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AndroidEnumCodeGenerateQuery))]
    public class When_android_enum_code_generate
    {
        #region Establish value

        enum MyEnum
        {
            Value = 1, 

            Value2 = 2, 

            Value3 = 3
        }

        static MockMessage<AndroidEnumCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidEnumCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models")
                                                                                                                .Tuning(r => r.Type, typeof(MyEnum)));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate",  typeof(When_android_enum_code_generate).Name));

                                  mockQuery = MockQuery<AndroidEnumCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, query.Type), new Dictionary<GetNameFromTypeQuery.ModeOf, string>() { { GetNameFromTypeQuery.ModeOf.Enum, "MyEnum" } });
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}