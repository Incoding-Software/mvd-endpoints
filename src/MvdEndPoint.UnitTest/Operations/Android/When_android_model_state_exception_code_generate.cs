namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AndroidModelStateExceptionCodeGenerateQuery))]
    public class When_android_model_state_exception_code_generate
    {
        #region Establish value

        static MockMessage<AndroidModelStateExceptionCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<AndroidModelStateExceptionCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models"));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_model_state_exception_code_generate).Name));

                                      mockQuery = MockQuery<AndroidModelStateExceptionCodeGenerateQuery, string>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}