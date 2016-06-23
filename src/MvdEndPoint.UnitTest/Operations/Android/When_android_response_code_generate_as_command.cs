namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AndroidResponseCodeGenerateQuery))]
    public class When_android_response_code_generate_as_command
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(AddCustomerCommand)));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate",  typeof(When_android_response_code_generate_as_command).Name));

                                  mockQuery = MockQuery<AndroidResponseCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(AddCustomerCommand))),
                                                     Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "AddCustomerCommand")
                                                                                                                        .Tuning(r => r.IsCommand, true)                                                                                                                        
                                                                                                                        .Tuning(r => r.Namespace, "com.qabenchmarking.android.models")))
                                          .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, query.Type), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                                        {
                                                                                                                                                                                { GetNameFromTypeQuery.ModeOf.Response, "AddCustomerResponse" }
                                                                                                                                                                        });
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Fake classes

        class AddCustomerCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<AndroidResponseCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}