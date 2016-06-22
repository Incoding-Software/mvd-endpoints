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

    [Subject(typeof(AndroidListenerCodeGeneratorQuery))]
    public class When_android_listener_code_generator
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Nested classes

            internal class Response { }

            #endregion

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Verify(bool isArray, string fileName)
        {
            var query = Pleasure.Generator.Invent<AndroidListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate",  fileName));

            var mockQuery = MockQuery<AndroidListenerCodeGeneratorQuery, string>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), isArray)
                    .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)),
                               Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models")
                                                                                                  .Tuning(r => r.Package, "com.qabenchmarking.android.models.FakeQuery")))
                    .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                         {
                                                                                                                                                                 { GetNameFromTypeQuery.ModeOf.Listener, "IFakeQueryListener" },
                                                                                                                                                                 { GetNameFromTypeQuery.ModeOf.Response, "FakeQueryResponse" },
                                                                                                                                                         });
            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        It should_be_verify = () => Verify(isArray: false, fileName: "When_android_listener_code_generator_for_query");

        It should_be_verify_as_array = () => Verify(isArray: true, fileName: "When_android_listener_code_generator_for_query_with_array");
    }
}