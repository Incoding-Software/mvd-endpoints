namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(MessageToPackageQuery))]
    public class When_message_to_package_query
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

        static MockMessage<MessageToPackageQuery, byte[]> mockQuery;

        static byte[] expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<MessageToPackageQuery>(dsl => dsl.Tuning(r => r.AssemblyQualifiedName, typeof(FakeQuery).AssemblyQualifiedName));
                                      expected = Pleasure.Generator.Bytes();

                                      string requestContent = Pleasure.Generator.String();
                                      string listenerContent = Pleasure.Generator.String();
                                      string taskContent = Pleasure.Generator.String();
                                      string responseContent = Pleasure.Generator.String();

                                      mockQuery = MockQuery<MessageToPackageQuery, byte[]>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)), "Request")
                                              .StubQuery(Pleasure.Generator.Invent<RequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                       .Tuning(r => r.BaseUrl, query.BaseUrl)), requestContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Listener)), "Listener")
                                              .StubQuery(Pleasure.Generator.Invent<ListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))), listenerContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Task)), "Task")
                                              .StubQuery(Pleasure.Generator.Invent<TaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))), taskContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)), "Response")
                                              .StubQuery(Pleasure.Generator.Invent<ResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))), responseContent)
                                              .StubQuery(Pleasure.Generator.Invent<ToZipQuery>(dsl => dsl.Tuning(r => r.Entries, new Dictionary<string, string>
                                                                                                                                     {
                                                                                                                                             { "Request.java", requestContent },
                                                                                                                                             { "Listener.java", listenerContent },
                                                                                                                                             { "Task.java", taskContent },
                                                                                                                                             { "Response.java", responseContent },
                                                                                                                                     })), expected);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}