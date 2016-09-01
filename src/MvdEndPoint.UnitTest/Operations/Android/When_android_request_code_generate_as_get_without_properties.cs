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

    [Subject(typeof(AndroidRequestCodeGenerateQuery))]
    public class When_android_request_code_generate_as_get_without_properties
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery)));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate",  typeof(When_android_request_code_generate_as_get_without_properties).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "GetCustomerQuery")
                                                                                                                .Tuning(r => r.IsCommand, true)                                                                                                                
                                                                                                                .Tuning(r => r.Namespace, "com.qabenchmarking.android.models"));
                                  mockQuery = MockQuery<AndroidRequestCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))),
                                                     meta)
                                          .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, query.Type), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                                        {
                                                                                                                                                                                { GetNameFromTypeQuery.ModeOf.Request, "GetCustomerRequest" },
                                                                                                                                                                                { GetNameFromTypeQuery.ModeOf.Listener, "IFakeOn" },
                                                                                                                                                                                { GetNameFromTypeQuery.ModeOf.Response, "Response" }
                                                                                                                                                                        })
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Tuning(r => r.Language, Language.JavaCE)
                                                                                                                      .Tuning(r => r.IsCommand, meta.IsCommand)
                                                                                                                      .Tuning(r => r.Type, query.Type), new List<GetPropertiesQuery.Response>());
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Fake classes

        class GetCustomerQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<AndroidRequestCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}