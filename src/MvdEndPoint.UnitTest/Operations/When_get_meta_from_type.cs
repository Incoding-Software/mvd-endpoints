namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.ServiceModel;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetMetaFromTypeQuery))]
    public class When_get_meta_from_type
    {
        #region Establish value

        static MockMessage<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response> mockQuery;

        #endregion

        private static bool asArray;

        private static bool asImage;

        private static bool isCommand;

        Establish establish = () =>
                              {
                                  GetMetaFromTypeQuery query = Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand)));

                                  asArray = Pleasure.Generator.Bool();
                                  asImage = Pleasure.Generator.Bool();
                                  isCommand = Pleasure.Generator.Bool();
                                  mockQuery = MockQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>
                                          .When(query)
                                          .StubQuery<HasQueryResponseAsArrayQuery, bool>(dsl => dsl.Tuning(s => s.Type, query.Type), asArray)
                                          .StubQuery<HasQueryResponseAsImageQuery, bool>(dsl => dsl.Tuning(s => s.Type, query.Type), asImage)
                                          .StubQuery<IsCommandTypeQuery, bool>(dsl => dsl.Tuning(s => s.Type, query.Type), isCommand);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(response => response.ShouldEqualWeak(new
                                                                                                    {
                                                                                                            Package = "Custom.Namespace.FakeCommand",
                                                                                                            ResponseAsArray = asArray,
                                                                                                            ResponseAsImage = asImage,
                                                                                                            IsCommand = isCommand,
                                                                                                            IsNotifyPropertyChanged = true,
                                                                                                            Namespace = "Incoding",
                                                                                                            Name = "FakeCommand"
                                                                                                    }));

        [ServiceContract(Namespace = "Custom.Namespace")]
        class FakeCommand { }
    }
}