namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SyncEndpointCommand.GetEndpointsQuery))]
    public class When_get_endpoints
    {
        #region Establish value

        static MockMessage<SyncEndpointCommand.GetEndpointsQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response>> mockQuery;

        #endregion

        Establish establish = () =>
                              {
                                  SyncEndpointCommand.GetEndpointsQuery query = Pleasure.Generator.Invent<SyncEndpointCommand.GetEndpointsQuery>();

                                  mockQuery = MockQuery<SyncEndpointCommand.GetEndpointsQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response>>
                                          .When(query);
                                  
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(response => response.Count.ShouldEqual(1));
    }
}