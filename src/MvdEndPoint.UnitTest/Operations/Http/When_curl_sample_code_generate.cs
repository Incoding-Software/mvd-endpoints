namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CurlSampleCodeGenerateQuery))]
    public class When_curl_sample_code_generate
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<CurlSampleCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Instance, Pleasure.Generator.Invent<GetCustomerQuery>(factoryDsl => factoryDsl.Tuning(r => r.Name, "ValueOfName")
                                                                                                                                                                                                            .Tuning(s => s.Number, 1))));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", typeof(When_curl_sample_code_generate).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "GetCustomerQuery"));

                                  var response = Pleasure.Generator.Invent<GetUriByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Scheme, "http")
                                                                                                                 .Tuning(r => r.Verb, "POST")
                                                                                                                 .Tuning(r => r.Url, "Dispatcher")
                                                                                                                 .Tuning(r => r.Authority, "localhost"));
                                  mockQuery = MockQuery<CurlSampleCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery<GetUriByTypeQuery, GetUriByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery)), response)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))), meta);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Fake classes

        class GetCustomerQuery : QueryBase<string>
        {
            public string Name { get; set; }

            public int Number { get; set; }

            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<CurlSampleCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}