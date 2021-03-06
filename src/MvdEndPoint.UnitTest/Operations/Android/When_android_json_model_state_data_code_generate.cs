﻿namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AndroidJsonModelStateDataCodeGenerateQuery))]
    public class When_android_json_model_state_data_code_generate
    {
        #region Establish value

        static MockMessage<AndroidJsonModelStateDataCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<AndroidJsonModelStateDataCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models"));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate",  typeof(When_android_json_model_state_data_code_generate).Name));

                                      mockQuery = MockQuery<AndroidJsonModelStateDataCodeGenerateQuery, string>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}