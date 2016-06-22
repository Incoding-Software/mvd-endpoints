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

    [Subject(typeof(WPGenerateCommonFileQuery))]
    public class When_wp_generate_common_class
    {
        Establish establish = () =>
                              {
                                  WPGenerateCommonFileQuery query = Pleasure.Generator.Invent<WPGenerateCommonFileQuery>(dsl => dsl.Tuning(r => r.Type, typeof(string)));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate",  typeof(When_wp_generate_common_class).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "MvdEndPoint.UnitTest")
                                                                                                                .Tuning(r => r.Name, "FakeClass"));
                                  mockQuery = MockQuery<WPGenerateCommonFileQuery, string>
                                          .When(query)
                                          .StubQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Type, query.Type), meta)
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Tuning(r => r.Type, query.Type)
                                                                                                                      .Tuning(r => r.IsCommand, false)
                                                                                                                      .Tuning(r => r.Device, DeviceOfType.WP)
                                                                                                            , new List<GetPropertiesQuery.Response>()
                                                                                                              {
                                                                                                                      new GetPropertiesQuery.Response() { Type = "String", Name = "Login" },
                                                                                                                      new GetPropertiesQuery.Response() { Type = "Int32", Name = "Count" },
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