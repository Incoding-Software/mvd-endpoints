namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using System.Collections.Generic;
    using CloudIn.Domain.Endpoint;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetMessagesQuery))]
    public class When_get_messages
    {
        Establish establish = () =>
                              {
                                  GetMessagesQuery query = Pleasure.Generator.Invent<GetMessagesQuery>();
                                  expected = Pleasure.ToArray(Pleasure.Generator.Invent<Message>());

                                  mockQuery = MockQuery<GetMessagesQuery, List<GetMessagesQuery.Response>>
                                          .When(query)
                                          .StubQuery(entities: expected);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(response => response.ShouldEqualWeakEach(expected));

        #region Establish value

        static MockMessage<GetMessagesQuery, List<GetMessagesQuery.Response>> mockQuery;

        static Message[] expected;

        #endregion
    }
}