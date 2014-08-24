namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(AndroidListenerCodeGeneratorQuery))]
    public class When_android_listener_code_generator_for_query_with_array
    {
        #region Fake classes

        class FakeQuery : QueryBase<List<FakeQuery.Response>>
        {
            #region Nested classes

            internal class Response { }

            #endregion

            protected override List<Response> ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<AndroidListenerCodeGeneratorQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<AndroidListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_listener_code_generator_for_query_with_array).Name));

                                      mockQuery = MockQuery<AndroidListenerCodeGeneratorQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)),
                                                         Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models")
                                                                                                                            .Tuning(r => r.Package, "com.qabenchmarking.android.models.FakeQuery")))
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Listener)), "IFakeQueryListener")
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)), "FakeQueryResponse");
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}