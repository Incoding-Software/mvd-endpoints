namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MessagesToPackageQuery.AsAndroidQuery))]
    public class When_message_to_package_as_android_query
    {
        It should_be_with_namespace = () => Run(typeof(FakeQuery), "Incoding");

        #region Establish value

        static void Run(Type mainType, string @namespace)
        {
            var query = Pleasure.Generator.Invent<MessagesToPackageQuery.AsAndroidQuery>(dsl => dsl.Tuning(r => r.Types, mainType.AssemblyQualifiedName.Replace("PublicKeyToken=null,", "PublicKeyToken=null|")
                                                                                                                                 .Split("|".ToCharArray())
                                                                                                                                 .Select(Type.GetType)
                                                                                                                                 .Where(r => r.HasAttribute<ServiceContractAttribute>())
                                                                                                                                 .ToList()));
            var expected = Pleasure.Generator.Bytes();

            string requestContent = Pleasure.Generator.String();
            string listenerContent = Pleasure.Generator.String();
            string responseContent = Pleasure.Generator.String();
            string jsonModelStateClassContent = Pleasure.Generator.String();
            string modelStateExceptionContent = Pleasure.Generator.String();
            string incodingHelperContent = Pleasure.Generator.String();
            string enumContent = Pleasure.Generator.String();

            var metaFromType = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.IsCommand, false));
            var mockQuery = MockQuery<MessagesToPackageQuery.AsAndroidQuery, byte[]>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<AndroidIncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                           .Tuning(r => r.Namespace, @namespace)), incodingHelperContent)
                    .StubQuery(Pleasure.Generator.Invent<AndroidModelStateExceptionCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, @namespace)), modelStateExceptionContent)
                    .StubQuery(Pleasure.Generator.Invent<AndroidJsonModelStateDataCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, @namespace)), jsonModelStateClassContent)
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(OuterEnum))), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                   {
                                                                                                                                           { GetNameFromTypeQuery.ModeOf.Enum, "OuterEnum" }
                                                                                                                                   })
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                          {
                                                                                                                                  { GetNameFromTypeQuery.ModeOf.Request, "Request" },
                                                                                                                                  { GetNameFromTypeQuery.ModeOf.Listener, "Listener" },
                                                                                                                                  { GetNameFromTypeQuery.ModeOf.Response, "Response" },
                                                                                                                          })
                    .StubQuery(Pleasure.Generator.Invent<AndroidRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), requestContent)
                    .StubQuery(Pleasure.Generator.Invent<AndroidListenerCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), listenerContent)
                    .StubQuery(Pleasure.Generator.Invent<AndroidResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), responseContent)
                    .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, mainType)), metaFromType)
                    .StubQuery(Pleasure.Generator.Invent<AndroidEnumCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, @namespace)
                                                                                                 .Tuning(r => r.Type, typeof(OuterEnum))), enumContent)
                    .StubQuery(Pleasure.Generator.Invent<ToZipQuery>(dsl => dsl.Tuning(r => r.Entries, new Dictionary<string, string>
                                                                                                       {
                                                                                                               { "Incoding/IncodingHelper.java", incodingHelperContent },
                                                                                                               { "Incoding/ModelStateException.java", modelStateExceptionContent },
                                                                                                               { "Incoding/JsonModelStateData.java", jsonModelStateClassContent },
                                                                                                               { "Incoding/Listener.java".F(mainType.Name), listenerContent },
                                                                                                               { "Incoding/Request.java".F(mainType.Name), requestContent },
                                                                                                               { "Incoding/Response.java".F(mainType.Name), responseContent },
                                                                                                               { "Incoding/OuterEnum.java".F(mainType.Name), enumContent },
                                                                                                       })), expected);
            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(expected);
        }

        #endregion

        #region Fake classes

        [ServiceContract(Namespace = "incoding.test")]
        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }

            #region Nested classes

            internal class Response
            {
                #region Properties

                public OuterEnum Value { get; set; }

                #endregion
            }

            #endregion
        }

        [ServiceContract()]
        class FakeWithoutNamespaceQuery : QueryBase<FakeWithoutNamespaceQuery.Response>
        {
            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }

            #region Nested classes

            internal class Response
            {
                #region Properties

                public OuterEnum Value { get; set; }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}