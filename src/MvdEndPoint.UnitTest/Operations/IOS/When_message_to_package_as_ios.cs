namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(MessageToPackageAsIosQuery))]
    public class When_message_to_package_as_ios
    {
        #region Fake classes

        [ServiceContract]
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

        static MockMessage<MessageToPackageAsIosQuery, byte[]> mockQuery;

        static byte[] expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var type = typeof(FakeQuery);
                                      var query = Pleasure.Generator.Invent<MessageToPackageAsIosQuery>(dsl => dsl.Tuning(r => r.Types, new List<Type> { type }));
                                      expected = Pleasure.Generator.Bytes();

                                      string requestHContent = Pleasure.Generator.String();
                                      string requestMContent = Pleasure.Generator.String();
                                      string responseHContent = Pleasure.Generator.String();
                                      string responseMContent = Pleasure.Generator.String();
                                      string modelStateHContent = Pleasure.Generator.String();
                                      string modelStateMContent = Pleasure.Generator.String();
                                      string incodingHelperHContent = Pleasure.Generator.String();
                                      string incodingHelperMContent = Pleasure.Generator.String();

                                      mockQuery = MockQuery<MessageToPackageAsIosQuery, byte[]>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<HasQueryResponseAsImageQuery>(dsl => dsl.Tuning(r => r.Type, type)), (IncBoolResponse)false)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)), "Request")
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)), "Response")
                                              .StubQuery(Pleasure.Generator.Invent<IosRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                          .Tuning(r => r.File, FileOfIos.H)), requestHContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                          .Tuning(r => r.File, FileOfIos.M)), requestMContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                           .Tuning(r => r.File, FileOfIos.H)), responseHContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                           .Tuning(r => r.File, FileOfIos.M)), responseMContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosIncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                                                 .Tuning(r => r.File, FileOfIos.H)
                                                                                                                                 .Tuning(r => r.Imports, new[] { "ModelStateException", "Request", "Response" }.ToList())), incodingHelperHContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosIncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                                                 .Tuning(r => r.File, FileOfIos.M)), incodingHelperMContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                           .Tuning(r => r.File, FileOfIos.M)), responseMContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosModelStateExceptionCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, FileOfIos.H)), modelStateHContent)
                                              .StubQuery(Pleasure.Generator.Invent<IosModelStateExceptionCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, FileOfIos.M)), modelStateMContent)
                                              .StubQuery(Pleasure.Generator.Invent<ToZipQuery>(dsl => dsl.Tuning(r => r.Entries, new Dictionary<string, string>
                                                                                                                                     {
                                                                                                                                             { "Request.h", requestHContent },
                                                                                                                                             { "Response.h", responseHContent },
                                                                                                                                             { "Request.m", requestMContent },
                                                                                                                                             { "Response.m", responseMContent },
                                                                                                                                             { "IncodingHelper.h", incodingHelperHContent },
                                                                                                                                             { "IncodingHelper.m", incodingHelperMContent },
                                                                                                                                             { "ModelStateException.h", modelStateHContent },
                                                                                                                                             { "ModelStateException.m", modelStateMContent },
                                                                                                                                     })), expected);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}