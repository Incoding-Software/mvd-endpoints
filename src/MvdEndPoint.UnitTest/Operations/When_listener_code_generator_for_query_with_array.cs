namespace MvdEndPoint.UnitTest
{
    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    [Subject(typeof(ListenerCodeGeneratorQuery))]
    public class When_listener_code_generator_for_query_with_array
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

        static MockMessage<ListenerCodeGeneratorQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<ListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));
                                      expected = @"
public interface FakeQueryListener {
    	void Success(FakeQueryResponse[] response);
	
}";

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