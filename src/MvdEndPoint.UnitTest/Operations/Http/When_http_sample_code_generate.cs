namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HttpSampleCodeGenerateQuery))]
    public class When_http_sample_code_generate
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<HttpSampleCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Instance, Pleasure.Generator.Invent<GetCustomerQuery>()));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", typeof(When_http_sample_code_generate).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "GetCustomerQuery"));

                                  var response = Pleasure.Generator.Invent<GetUriByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Scheme, "http")
                                                                                                                 .Tuning(r => r.Verb, "POST")
                                                                                                                 .Tuning(r => r.Url, "Dispatcher")
                                                                                                                 .Tuning(r => r.Authority, "localhost"));
                                  mockQuery = MockQuery<HttpSampleCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Empty(r => r.Language)
                                                                                                                      .Tuning(r => r.IsCommand, meta.IsCommand)
                                                                                                                      .Tuning(s => s.Type, typeof(GetCustomerQuery)), new List<GetPropertiesQuery.Response>()
                                                                                                                                                                      {
                                                                                                                                                                              new GetPropertiesQuery.Response() { Name = "Name", Type = "String" },
                                                                                                                                                                              new GetPropertiesQuery.Response() { Name = "Count", Type = "Int" }
                                                                                                                                                                      })
                                          .StubQuery<GetUriByTypeQuery, GetUriByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery)), response)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))), meta);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Fake classes

        class GetCustomerQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<HttpSampleCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}