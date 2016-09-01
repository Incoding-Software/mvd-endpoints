namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetUriByTypeQuery))]
    public class When_get_uri_by_type
    {
        Establish establish = () =>
                              {
                                  GetUriByTypeQuery query = Pleasure.Generator.Invent<GetUriByTypeQuery>(dsl => dsl.Tuning(s => s.Type, typeof(GetUriByTypeQuery)));
                                  expected = Pleasure.Generator.Invent<GetUriByTypeQuery.Response>();

                                  mockQuery = MockQuery<GetUriByTypeQuery, GetUriByTypeQuery.Response>
                                          .When(query)
                                          .StubQuery<IsCommandTypeQuery, bool>(dsl => dsl.Tuning(s => s.Type, query.Type), true);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Establish value

        static MockMessage<GetUriByTypeQuery, GetUriByTypeQuery.Response> mockQuery;

        static GetUriByTypeQuery.Response expected;

        #endregion
    }
}