namespace MvdEndPoint.UnitTest
{
    using System.ServiceModel;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    [Subject(typeof(GetMetaFromTypeQuery))]
    public class When_get_meta_from_type_without_namespace
    {
        #region Establish value

        static MockMessage<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response> mockQuery;

        #endregion

        Establish establish = () =>
                              {
                                  GetMetaFromTypeQuery query = Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand)));

                                  mockQuery = MockQuery<GetMetaFromTypeQuery, GetMetaFromTypeQuery.Response>
                                          .When(query);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(response => response.ShouldEqualWeak(new
                                                                                                    {
                                                                                                            Package = "MvdEndPoint.UnitTest.FakeCommand",
                                                                                                            Namespace = "MvdEndPoint.UnitTest",
                                                                                                            Name = "FakeCommand"
                                                                                                    }));

        [ServiceContract]
        class FakeCommand { }
    }
}