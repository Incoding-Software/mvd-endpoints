namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(MessagesToPackageQuery))]
    public class When_message_to_package_query
    {
        #region Fake classes

        [ServiceContract(Namespace = IncodingTest)]
        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Nested classes

            internal class Response
            {
                #region Properties

                public OuterEnum Value { get; set; }

                #endregion
            }

            #endregion

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        public const string IncodingTest = "incoding.test";

        static MockMessage<MessagesToPackageQuery, byte[]> mockQuery;

        static byte[] expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var mainType = typeof(FakeQuery);
                                      var query = Pleasure.Generator.Invent<MessagesToPackageQuery>(dsl => dsl.Tuning(r => r.Names, mainType.AssemblyQualifiedName));
                                      expected = Pleasure.Generator.Bytes();

                                      string requestContent = Pleasure.Generator.String();
                                      string listenerContent = Pleasure.Generator.String();
                                      string taskContent = Pleasure.Generator.String();
                                      string responseContent = Pleasure.Generator.String();
                                      string jsonModelStateClassContent = Pleasure.Generator.String();
                                      string modelStateExceptionContent = Pleasure.Generator.String();
                                      string incodingHelperContent = Pleasure.Generator.String();
                                      string enumContent = Pleasure.Generator.String();

                                      var metaFromType = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>();
                                      mockQuery = MockQuery<MessagesToPackageQuery, byte[]>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<IncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, IncodingTest)), incodingHelperContent)
                                              .StubQuery(Pleasure.Generator.Invent<ModelStateExceptionCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, IncodingTest)), modelStateExceptionContent)
                                              .StubQuery(Pleasure.Generator.Invent<JsonModelStateDataCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, IncodingTest)), jsonModelStateClassContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)), "Request")
                                              .StubQuery(Pleasure.Generator.Invent<RequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                       .Tuning(r => r.BaseUrl, query.BaseUrl)), requestContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Listener)), "Listener")
                                              .StubQuery(Pleasure.Generator.Invent<ListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), listenerContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Task)), "Task")
                                              .StubQuery(Pleasure.Generator.Invent<TaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), taskContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)), "Response")
                                              .StubQuery(Pleasure.Generator.Invent<ResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), responseContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), metaFromType)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(OuterEnum))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Enum)), "OuterEnum")
                                              .StubQuery(Pleasure.Generator.Invent<EnumCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Package, metaFromType.Package)
                                                                                                                    .Tuning(r => r.Type, typeof(OuterEnum))), enumContent)
                                              .StubQuery(Pleasure.Generator.Invent<ToZipQuery>(dsl => dsl.Tuning(r => r.Entries, new Dictionary<string, string>
                                                                                                                                     {
                                                                                                                                             { "IncodingHelper.java", incodingHelperContent },
                                                                                                                                             { "ModelStateException.java", modelStateExceptionContent },
                                                                                                                                             { "JsonModelStateData.java", jsonModelStateClassContent },
                                                                                                                                             { "Request.java", requestContent },
                                                                                                                                             { "Listener.java", listenerContent },
                                                                                                                                             { "Task.java", taskContent },
                                                                                                                                             { "Response.java", responseContent },
                                                                                                                                             { "OuterEnum.java", enumContent },
                                                                                                                                     })), expected);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}