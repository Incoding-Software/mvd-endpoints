namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetMessagesQuery))]
    public class When_get_messages
    {
        Establish establish = () =>
                              {
                                  GetMessagesQuery query = Pleasure.Generator.Invent<GetMessagesQuery>();
                                  uniqueGroup = Pleasure.Generator.Invent<Message.Group>();
                                  shareGroup = Pleasure.Generator.Invent<Message.Group>(s => s.Tuning(r => r.Id, "1"));
                                  expected = Pleasure.ToArray(Pleasure.Generator.Invent<Message>(dsl => dsl.Tuning(r => r.GroupKey, uniqueGroup)),
                                                              Pleasure.Generator.Invent<Message>(dsl => dsl.Tuning(r => r.GroupKey, shareGroup)),
                                                              Pleasure.Generator.Invent<Message>(dsl => dsl.Tuning(r => r.GroupKey, shareGroup)));

                                  uri = Pleasure.Generator.Invent<GetUriByTypeQuery.Response>();
                                  httpSample = Pleasure.Generator.String();
                                  androidSample = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetMessagesQuery, List<GetMessagesQuery.Response>>
                                          .When(query)
                                          .StubQuery<CreateByTypeQuery.FindTypeByName, Type>(dsl => dsl.Tuning(r => r.Type, expected[0].Type), typeof(FakeCommand))
                                          .StubQuery<CreateByTypeQuery.FindTypeByName, Type>(dsl => dsl.Tuning(r => r.Type, expected[1].Type), typeof(FakeCommand))
                                          .StubQuery<CreateByTypeQuery.FindTypeByName, Type>(dsl => dsl.Tuning(r => r.Type, expected[2].Type), typeof(FakeCommand2))
                                          .StubQuery<GetUriByTypeQuery, GetUriByTypeQuery.Response>(dsl => dsl.Tuning(s => s.Type, typeof(FakeCommand)), uri)
                                          .StubQuery<GetUriByTypeQuery, GetUriByTypeQuery.Response>(dsl => dsl.Tuning(s => s.Type, typeof(FakeCommand2)), uri)
                                          .StubQuery<HttpSampleCodeGenerateQuery, string>(dsl => dsl.Tuning(s => s.Instance, new FakeCommand()), httpSample)
                                          .StubQuery<HttpSampleCodeGenerateQuery, string>(dsl => dsl.Tuning(s => s.Instance, new FakeCommand2()), httpSample)
                                          .StubQuery<AndroidSampleCodeGenerateQuery, string>(dsl => dsl.Tuning(s => s.Instance, new FakeCommand()), androidSample)
                                          .StubQuery<AndroidSampleCodeGenerateQuery, string>(dsl => dsl.Tuning(s => s.Instance, new FakeCommand2()), androidSample)
                                          .StubQuery(orderSpecification: new Message.Order.Default(),
                                                     entities: expected);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(response =>
                                                               {
                                                                   Func<Message.Group, GetMessagesQuery.Response> createGr = @group =>
                                                                                                                             {
                                                                                                                                 return Pleasure.Generator.Invent<GetMessagesQuery.Response>(dsl => dsl.Tuning(s => s.Group, @group.Name)
                                                                                                                                                                                                       .Tuning(r => r.Id, @group.Name)
                                                                                                                                                                                                       .Tuning(r => r.Name, null)
                                                                                                                                                                                                       .Tuning(r => r.Description, @group.Description)
                                                                                                                                                                                                       .Tuning(r => r.IsGroup, true)
                                                                                                                                                                                                       .Tuning(r => r.EntityId, @group.Id)
                                                                                                                                                                                                       .Tuning(r => r.Host, null)
                                                                                                                                                                                                       .Tuning(r => r.Verb, null)
                                                                                                                                                                                                       .Tuning(r => r.Url, null)
                                                                                                                                                                                                       .Tuning(r => r.PropertiesOfRequest, null)
                                                                                                                                                                                                       .Tuning(r => r.PropertiesOfResponse, null)
                                                                                                                                                                                                       .Tuning(r => r.Result, null)
                                                                                                                                                                                                       .Tuning(r => r.SampleOfAndroid, null)
                                                                                                                                                                                                       .Tuning(r => r.SampleOfHttp, null));
                                                                                                                             };

                                                                   Func<Message, GetMessagesQuery.Response> createItem = message =>
                                                                                                                         {
                                                                                                                             return Pleasure.Generator.Invent<GetMessagesQuery.Response>(dsl => dsl.Tuning(s => s.Group, message.GroupKey.Name)
                                                                                                                                                                                                   .Tuning(r => r.Id, message.Name)
                                                                                                                                                                                                   .Tuning(r => r.Name, message.Name)
                                                                                                                                                                                                   .Tuning(r => r.Description, message.Description)
                                                                                                                                                                                                   .Tuning(r => r.IsGroup, false)
                                                                                                                                                                                                   .Tuning(r => r.EntityId, message.Id)
                                                                                                                                                                                                   .Tuning(r => r.Host, uri.Host)
                                                                                                                                                                                                   .Tuning(r => r.Verb, uri.Verb)
                                                                                                                                                                                                   .Tuning(r => r.Result, message.Result)
                                                                                                                                                                                                   .Tuning(r => r.Url, uri.Host + uri.Url)
                                                                                                                                                                                                   .Tuning(r => r.PropertiesOfRequest, new List<GetMessagesQuery.Response.Item>())
                                                                                                                                                                                                   .Tuning(r => r.PropertiesOfResponse, new List<GetMessagesQuery.Response.Item>())
                                                                                                                                                                                                   .Tuning(r => r.SampleOfAndroid, androidSample)
                                                                                                                                                                                                   .Tuning(r => r.SampleOfHttp, httpSample));
                                                                                                                         };

                                                                   response.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        createGr(uniqueGroup),
                                                                                                        createItem(expected[0]),
                                                                                                        createGr(shareGroup),
                                                                                                        createItem(expected[1]),
                                                                                                        createItem(expected[2])
                                                                                                });
                                                               });

        public class FakeCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class FakeCommand2 : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #region Establish value

        static MockMessage<GetMessagesQuery, List<GetMessagesQuery.Response>> mockQuery;

        static Message[] expected;

        private static Message.Group uniqueGroup;

        private static Message.Group shareGroup;

        private static string httpSample;

        private static string androidSample;

        private static GetUriByTypeQuery.Response uri;

        #endregion
    }
}