namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(WPGenerateQueryQuery))]
    public class When_wp_generate_query
    {
        Establish establish = () =>
                              {
                                  var type = typeof(FakeQuery);
                                  var query = Pleasure.Generator.Invent<WPGenerateQueryQuery>(dsl => dsl.Tuning(r => r.Type, type));

                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", typeof(When_wp_generate_query).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, type.Name)
                                                                                                                .Tuning(r => r.ResponseAsArray, true)
                                                                                                                .Tuning(s => s.IsNotifyPropertyChanged, false)
                                                                                                                .Tuning(r => r.Namespace, type.Namespace));
                                  var properties = Pleasure.ToList(Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Login")
                                                                                                                                    .Tuning(r => r.Attributes, GetPropertiesQuery.Response.OfAttributes.IsClass | GetPropertiesQuery.Response.OfAttributes.IsCanNull)
                                                                                                                                    .Tuning(r => r.Type, typeof(string).Name)),
                                                                   Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Count")
                                                                                                                                    .Tuning(r => r.Type, typeof(int).Name)
                                                                                                                                    .Tuning(r => r.Attributes, GetPropertiesQuery.Response.OfAttributes.IsCanNull)),
                                                                   Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Values")
                                                                                                                                    .Tuning(r => r.Attributes, GetPropertiesQuery.Response.OfAttributes.IsArray)
                                                                                                                                    .Tuning(r => r.Type, typeof(double).Name)));
                                  mockQuery = MockQuery<WPGenerateQueryQuery, string>
                                          .When(query)
                                          .StubQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Type, type), meta)
                                          .StubQuery<GetInnerResponseTypesQuery, Dictionary<string, List<GetPropertiesQuery.Response>>>(dsl => dsl.Tuning(r => r.Type, type), new Dictionary<string, List<GetPropertiesQuery.Response>>()
                                                                                                                                                                              {
                                                                                                                                                                                      { "MyClass", properties }
                                                                                                                                                                              })
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Tuning(r => r.Type, type)
                                                                                                                      .Tuning(r => r.IsCommand, meta.IsCommand)
                                                                                                                      .Tuning(r => r.Language, Language.Csharp),
                                                                                                            properties)
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery.Response))
                                                                                                                      .Tuning(r => r.IsCommand, meta.IsCommand)
                                                                                                                      .Tuning(r => r.Language, Language.Csharp),
                                                                                                            properties);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Fake classes

        [ExcludeFromCodeCoverage]
        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }

            #region Nested classes

            public class Response { }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<WPGenerateQueryQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}