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
        It should_be_with_namespace = () => { Run(typeof(FakeQuery), "incoding.test"); };

        It should_be_without_namespace = () => { Run(typeof(FakeWithoutNamespaceQuery), "MvdEndPoint.UnitTest"); };

        static void Run(Type mainType, string @namespace)
        {
            var query = Pleasure.Generator.Invent<MessageToPackageAsAndroidQuery>(dsl => dsl.Tuning(r => r.Types, mainType.AssemblyQualifiedName.Replace("PublicKeyToken=null,", "PublicKeyToken=null|")
                                                                                                                          .Split("|".ToCharArray())
                                                                                                                          .Select(Type.GetType)
                                                                                                                          .Where(r => r.HasAttribute<ServiceContractAttribute>())
                                                                                                                          .ToList()));
            var expected = Pleasure.Generator.Bytes();

            string requestContent = Pleasure.Generator.String();
            string listenerContent = Pleasure.Generator.String();
            string taskContent = Pleasure.Generator.String();
            string responseContent = Pleasure.Generator.String();
            string jsonModelStateClassContent = Pleasure.Generator.String();
            string modelStateExceptionContent = Pleasure.Generator.String();
            string incodingHelperContent = Pleasure.Generator.String();
            string enumContent = Pleasure.Generator.String();

            var metaFromType = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>();
            var mockQuery = MockQuery<MessageToPackageAsAndroidQuery, byte[]>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<AndroidIncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                           .Tuning(r => r.Namespace, @namespace)), incodingHelperContent)
                    .StubQuery(Pleasure.Generator.Invent<AndroidModelStateExceptionCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, @namespace)), modelStateExceptionContent)
                    .StubQuery(Pleasure.Generator.Invent<AndroidJsonModelStateDataCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, @namespace)), jsonModelStateClassContent)
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
                                                                                                               { "{0}/Request.java".F(mainType.Name), requestContent },
                                                                                                               { "{0}/Listener.java".F(mainType.Name), listenerContent },
                                                                                                               { "{0}/Task.java".F(mainType.Name), taskContent },
                                                                                                               { "{0}/Response.java".F(mainType.Name), responseContent },
                                                                                                               { "{0}/OuterEnum.java".F(mainType.Name), enumContent },
                                                                                                       })), expected);
            mockQuery.Original.Execute();
            mockQuery.ShouldBeIsResult(expected);
        }

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