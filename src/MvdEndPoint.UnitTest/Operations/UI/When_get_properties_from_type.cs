namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery))]
    public class When_get_properties_from_type
    {
        #region Establish value

        static MockMessage<SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>> mockQuery;

        #endregion

        private static List<SyncEndpointCommand.GetEndpointsQuery.Response.Property> childrens;

        Establish establish = () =>
                              {
                                  SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery query = Pleasure.Generator.Invent<SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeModel)));
                                  childrens = new List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>()
                                              {
                                                      new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                      {
                                                              Type = typeof(string),
                                                              Name = "Value"
                                                      }
                                              };
                                  mockQuery = MockQuery<SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>>
                                          .When(query)
                                          .StubQuery<SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>>(dsl => dsl.Tuning(s => s.Type, typeof(FakeInnerModel))
                                                                                                                                                                                                .Tuning(s => s.IsWrite, query.IsWrite), childrens);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                                                                        {
                                                                                                                Type = typeof(string),
                                                                                                                Name = "Name"
                                                                                                        },
                                                                                                        new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                                                                        {
                                                                                                                Type = typeof(int),
                                                                                                                Name = "Value"
                                                                                                        },
                                                                                                        new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                                                                        {
                                                                                                                Type = typeof(FakeInnerModel),
                                                                                                                Name = "Model",
                                                                                                                Childrens = childrens
                                                                                                        },
                                                                                                }));

        public class FakeInnerModel
        {
            public string Value { get; set; }
        }

        public class FakeModel
        {
            public string Name { get; set; }

            public int Value { get; set; }

            public FakeInnerModel Model { get; set; }
        }
    }
}