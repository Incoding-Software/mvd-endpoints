namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AndroidIncodingHelperCodeGenerateQuery))]
    public class When_android_incoding_helper_code_generate
    {
        #region Establish value

        static MockMessage<AndroidIncodingHelperCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<AndroidIncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, "http://localhost:48801/")
                                          .Tuning(r => r.Namespace, "com.qabenchmarking.android.models"));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_incoding_helper_code_generate).Name));

                                      mockQuery = MockQuery<AndroidIncodingHelperCodeGenerateQuery, string>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}