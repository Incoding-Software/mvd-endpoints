namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetPropertiesQuery))]
    public class When_get_properties_by_type_issue_1
    {
        #region Establish value

        static MockMessage<GetPropertiesQuery, List<GetPropertiesQuery.Response>> mockQuery;

        #endregion

        Establish establish = () =>
                              {
                                  GetPropertiesQuery query = Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Type, typeof(PagingResult<TestResponse>)));

                                  mockQuery = MockQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>
                                          .When(query)
                                          .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(s => s.Type, typeof(TestResponse))
                                                                                                       .Tuning(r => r.Device, query.Device), Pleasure.Generator.String())
                                          .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(s => s.Type, typeof(PagingContainer))
                                                                                                       .Tuning(r => r.Device, query.Device), Pleasure.Generator.String());
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(result => result.ShouldEqualWeakEach(new List<GetPropertiesQuery.Response>()));

        public class TestResponse { }

        public class PagingContainer
        {
            public string Start { get; set; }

            public string End { get; set; }

            public string Total { get; set; }
        }

        public class PagingResult<TModel>
        {
            #region Properties

            public List<TModel> Items { get; set; }

            public PagingContainer Paging { get; set; }

            #endregion
        }
    }
}