namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetMetaFromTypeQuery))]
    public class When_meta_from_type_without_service_contract
    {
        Establish establish = () =>
                              {
                                  GetMetaFromTypeQuery query = Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeMetaClass)));
                                  expected = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "FakeMetaClass")
                                                                                                                .Tuning(r => r.IsCommand, false)
                                                                                                                .Tuning(s => s.IsNotifyPropertyChanged, true)
                                                                                                                .Tuning(r => r.ResponseAsArray, false)
                                                                                                                .Tuning(r => r.ResponseAsImage, false)
                                                                                                                .Tuning(r => r.Namespace, "Incoding"));

                                  mockQuery = MockQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>
                                          .When(query);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        public class FakeMetaClass { }

        #region Establish value

        static MockMessage<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response> mockQuery;

        static GetMetaFromTypeQuery.Response expected;

        #endregion
    }
}