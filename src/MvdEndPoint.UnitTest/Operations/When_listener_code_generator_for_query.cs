namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(ListenerCodeGeneratorQuery))]
    public class When_listener_code_generator_for_query
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

        static MockMessage<ListenerCodeGeneratorQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<ListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_listener_code_generator_for_query).Name));

                                      mockQuery = MockQuery<ListenerCodeGeneratorQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Listener)), "FakeQueryListener")
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)), "FakeQueryResponse");
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}