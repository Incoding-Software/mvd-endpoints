namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(WPGenerateHttpMessageQuery))]
    public class When_wp_generate_http_message
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<WPGenerateHttpMessageQuery>(dsl => dsl.Tuning(r => r.Namespace, "Project"));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", typeof(When_wp_generate_http_message).Name));

                                  mockQuery = MockQuery<WPGenerateHttpMessageQuery, string>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Establish value

        static MockMessage<WPGenerateHttpMessageQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}