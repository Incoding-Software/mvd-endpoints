namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

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
                                      var query = Pleasure.Generator.Invent<AndroidEnumCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Package, "com.qabenchmarking.android.models")
                                          .Tuning(r => r.Type, typeof(MyEnum)));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_enum_code_generate).Name));

                                      mockQuery = MockQuery<AndroidEnumCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Enum)), "MyEnum");
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}