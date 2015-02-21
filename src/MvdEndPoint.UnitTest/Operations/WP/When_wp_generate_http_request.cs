namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(WPGenerateHttpRequestQuery))]
    public class When_wp_generate_http_request
    {
        Establish establish = () =>
                              {
                                  WPGenerateHttpRequestQuery query = Pleasure.Generator.Invent<WPGenerateHttpRequestQuery>(dsl => dsl.Tuning(r => r.Url, "http://test.incoding.biz/ru"));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_wp_generate_http_request).Name));

                                  mockQuery = MockQuery<WPGenerateHttpRequestQuery, string>
                                          .When(query);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Establish value

        static MockMessage<WPGenerateHttpRequestQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}