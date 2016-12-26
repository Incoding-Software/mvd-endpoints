namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MessagesToPackageQuery.AsWPQuery))]
    public class When_message_to_package_as_wp
    {
        Establish establish = () =>
                              {
                                  var types = new[]
                                              {
                                                      typeof(string),
                                                      typeof(FakeQuery)
                                              }.ToList();
                                  MessagesToPackageQuery.AsWPQuery query = Pleasure.Generator.Invent<MessagesToPackageQuery.AsWPQuery>(dsl => dsl.Tuning(r => r.Types, types));
                                  expected = Pleasure.Generator.Invent<byte[]>();

                                  var httpMessageContent = Pleasure.Generator.String();
                                  var metaForString = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.IsCommand, true));
                                  var metaForInt = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.IsCommand, false));
                                  var commandContent = Pleasure.Generator.String();
                                  var queryContent = Pleasure.Generator.String();
                                  var dtContent = Pleasure.Generator.String();
                                  var charContent = Pleasure.Generator.String();
                                  var intContent = Pleasure.Generator.String();
                                  mockQuery = MockQuery<MessagesToPackageQuery.AsWPQuery, byte[]>
                                          .When(query)
                                          .StubQuery<WPGenerateHttpMessageQuery, string>(dsl => dsl.Tuning(r => r.Namespace, query.Namespace), httpMessageContent)
                                          .StubQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Type, typeof(string)), metaForString)
                                          .StubQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)), metaForInt)
                                          .StubQuery<WPGenerateCommandQuery, string>(dsl => dsl.Tuning(r => r.Type, typeof(string)), commandContent)
                                          .StubQuery<WPGenerateQueryQuery, string>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)), queryContent)
                                          .StubQuery<GetShareTypeFromTypeQuery, List<Type>>(dsl => dsl.Tuning(r => r.Type, typeof(long)), new[]
                                                                                                                                          {
                                                                                                                                                  typeof(DateTime),
                                                                                                                                                  typeof(int),
                                                                                                                                                  typeof(char),
                                                                                                                                          }.ToList())
                                          .StubQuery<WPGenerateCommonFileQuery, string>(dsl => dsl.Tuning(r => r.Type, typeof(DateTime)), dtContent)
                                          .StubQuery<WPGenerateCommonFileQuery, string>(dsl => dsl.Tuning(r => r.Type, typeof(int)), intContent)
                                          .StubQuery<WPGenerateCommonFileQuery, string>(dsl => dsl.Tuning(r => r.Type, typeof(char)), charContent)
                                          .StubQuery(Pleasure.Generator.Invent<ToZipQuery>(dsl => dsl.Tuning(r => r.Entries, new Dictionary<string, string>
                                                                                                                             {
                                                                                                                                     { "HttpMessageBase.cs", httpMessageContent },
                                                                                                                                     { "{0}.cs".F(metaForString.Name), commandContent },
                                                                                                                                     { "{0}.cs".F(metaForInt.Name), queryContent },
                                                                                                                                     { "{0}.cs".F(typeof(DateTime).Name), dtContent },
                                                                                                                                     { "{0}.cs".F(typeof(int).Name), intContent },
                                                                                                                                     { "{0}.cs".F(typeof(char).Name), charContent }
                                                                                                                             })), expected);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        public class FakeQuery : QueryBase<long>
        {
            protected override long ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #region Establish value

        static MockMessage<MessagesToPackageQuery.AsWPQuery, byte[]> mockQuery;

        static byte[] expected;

        #endregion
    }
}