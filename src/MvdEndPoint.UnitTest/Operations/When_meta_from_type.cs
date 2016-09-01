namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.ServiceModel;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetMetaFromTypeQuery))]
    public class When_meta_from_type
    {
        Establish establish = () =>
                              {
                                  GetMetaFromTypeQuery query = Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeMetaClass)));
                                  expected = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "FakeMetaClass")
                                                                                                                .Tuning(s => s.IsNotifyPropertyChanged, true)
                                                                                                                .Tuning(r => r.Namespace, "Incoding"));

                                  mockQuery = MockQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>
                                          .When(query)
                                          .StubQuery<HasQueryResponseAsArrayQuery, bool>(dsl => dsl.Tuning(r => r.Type, query.Type), expected.ResponseAsArray)
                                          .StubQuery<IsCommandTypeQuery, bool>(dsl => dsl.Tuning(r => r.Type, query.Type), expected.IsCommand)
                                          .StubQuery<HasQueryResponseAsImageQuery, bool>(dsl => dsl.Tuning(r => r.Type, query.Type), expected.ResponseAsImage);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        [ServiceContract()]
        public class FakeMetaClass { }

        #region Establish value

        static MockMessage<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response> mockQuery;

        static GetMetaFromTypeQuery.Response expected;

        #endregion
    }
}