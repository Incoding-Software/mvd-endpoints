namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(MessageToPackageAsAndroidQuery))]
    public class When_message_to_package_as_android_query
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

        static MockMessage<MessageToPackageAsAndroidQuery, byte[]> mockQuery;

        static byte[] expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var mainType = typeof(FakeQuery);
                                      var query = Pleasure.Generator.Invent<MessageToPackageAsAndroidQuery>(dsl => dsl.Tuning(r => r.Types, mainType.AssemblyQualifiedName.Replace("PublicKeyToken=null,", "PublicKeyToken=null|")
                                                                                                                                                    .Split("|".ToCharArray())
                                                                                                                                                    .Select(Type.GetType)
                                                                                                                                                    .Where(r => r.HasAttribute<ServiceContractAttribute>())
                                                                                                                                                    .ToList()));
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
                                      mockQuery = MockQuery<MessageToPackageAsAndroidQuery, byte[]>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<AndroidIncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                                                     .Tuning(r => r.Namespace, IncodingTest)), incodingHelperContent)
                                              .StubQuery(Pleasure.Generator.Invent<AndroidModelStateExceptionCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, IncodingTest)), modelStateExceptionContent)
                                              .StubQuery(Pleasure.Generator.Invent<AndroidJsonModelStateDataCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, IncodingTest)), jsonModelStateClassContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)), "Request")
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Listener)), "Listener")
                                              .StubQuery(Pleasure.Generator.Invent<AndroidRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), requestContent)
                                              .StubQuery(Pleasure.Generator.Invent<AndroidListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), listenerContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Task)), "Task")
                                              .StubQuery(Pleasure.Generator.Invent<AndroidTaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), taskContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)), "Response")
                                              .StubQuery(Pleasure.Generator.Invent<AndroidResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), responseContent)
                                              .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), metaFromType)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(OuterEnum))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Enum)), "OuterEnum")
                                              .StubQuery(Pleasure.Generator.Invent<AndroidEnumCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Package, metaFromType.Package)
                                                                                                                           .Tuning(r => r.Type, typeof(OuterEnum))), enumContent)
                                              .StubQuery(Pleasure.Generator.Invent<ToZipQuery>(dsl => dsl.Tuning(r => r.Entries, new Dictionary<string, string>
                                                                                                                                     {
                                                                                                                                             { "Incoding/IncodingHelper.java", incodingHelperContent },
                                                                                                                                             { "Incoding/ModelStateException.java", modelStateExceptionContent },
                                                                                                                                             { "Incoding/JsonModelStateData.java", jsonModelStateClassContent },
                                                                                                                                             { "FakeQuery/Request.java", requestContent },
                                                                                                                                             { "FakeQuery/Listener.java", listenerContent },
                                                                                                                                             { "FakeQuery/Task.java", taskContent },
                                                                                                                                             { "FakeQuery/Response.java", responseContent },
                                                                                                                                             { "FakeQuery/OuterEnum.java", enumContent },
                                                                                                                                     })), expected);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}