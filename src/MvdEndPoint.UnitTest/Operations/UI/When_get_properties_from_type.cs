namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery))]
    public class When_get_properties_from_type
    {
        public enum FakeEnum
        {
            Value,

            Value2
        }

        #region Establish value

        static MockMessage<SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>> mockQuery;

        #endregion

        private static List<SyncEndpointCommand.GetEndpointsQuery.Response.Property> childrens;

        private static KeyValueVm[] valuesForEnum;

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
                                  valuesForEnum = Pleasure.Generator.Invent<KeyValueVm[]>();
                                  mockQuery = MockQuery<SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>>
                                          .When(query)
                                          .StubQuery<GetEnumForDD, OptGroupVm>(dsl => dsl.Tuning(s => s.TypeId, typeof(FakeEnum).GUID.ToString()), new OptGroupVm("Test", valuesForEnum))
                                          .StubQuery<SyncEndpointCommand.GetEndpointsQuery.GetPropertiesFromTypeQuery, List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>>(dsl => dsl.Tuning(s => s.Type, typeof(FakeInnerModel))
                                                                                                                                                                                                .Tuning(s => s.IsWrite, query.IsWrite), childrens);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                                                                        {
                                                                                                                Type = typeof(string),
                                                                                                                Values = new List<string>(),
                                                                                                                Name = "Name"
                                                                                                        },
                                                                                                        new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                                                                        {
                                                                                                                Type = typeof(int),
                                                                                                                Values = new List<string>(),
                                                                                                                Name = "Value"
                                                                                                        },
                                                                                                        new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                                                                        {
                                                                                                                Type = typeof(FakeInnerModel),
                                                                                                                Name = "Model",
                                                                                                                Values = new List<string>(),
                                                                                                                Childrens = childrens
                                                                                                        },
                                                                                                        new SyncEndpointCommand.GetEndpointsQuery.Response.Property()
                                                                                                        {
                                                                                                                Type = typeof(FakeEnum),
                                                                                                                Name = "FakeEnum",
                                                                                                                Values = valuesForEnum.Select(s => s.Text).ToList(),
                                                                                                        }
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

            public FakeEnum FakeEnum { get; set; }
            
        }

        
    }
}