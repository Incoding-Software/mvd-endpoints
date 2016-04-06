namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(WPGenerateCommonFileQuery))]
    public class When_wp_generate_common_enum
    {
        public enum FakeEnum
        { }

        Establish establish = () =>
                              {
                                  WPGenerateCommonFileQuery query = Pleasure.Generator.Invent<WPGenerateCommonFileQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum)));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_wp_generate_common_enum).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "MvdEndPoint.UnitTest")
                                                                                                                .Tuning(r => r.Name, "FakeClass"));
                                  mockQuery = MockQuery<WPGenerateCommonFileQuery, string>
                                          .When(query)
                                          .StubQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Type, query.Type), meta)
                                          .StubQuery<GetValuesOfEnumQuery, List<GetValuesOfEnumQuery.Response>>(dsl => dsl.Tuning(r => r.Type, query.Type)
                                                                                                                , new List<GetValuesOfEnumQuery.Response>()
                                                                                                                  {
                                                                                                                          new GetValuesOfEnumQuery.Response() { AsInt = "1", AsString = "Value", Display = "Value 1" },
                                                                                                                          new GetValuesOfEnumQuery.Response() { AsInt = "2", AsString = "Value", Display = "" }
                                                                                                                  });
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Establish value

        static MockMessage<WPGenerateCommonFileQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}