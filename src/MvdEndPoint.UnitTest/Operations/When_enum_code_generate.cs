namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(EnumCodeGenerateQuery))]
    public class When_enum_code_generate
    {
        #region Establish value

        enum MyEnum
        {
            Value = 1, 

            Value2 = 2, 

            Value3 = 3
        }

        static MockMessage<EnumCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<EnumCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Package, "com.qabenchmarking.android.models")
                                          .Tuning(r => r.Type, typeof(MyEnum)));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_enum_code_generate).Name));

                                      mockQuery = MockQuery<EnumCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Enum)), "MyEnum");
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}