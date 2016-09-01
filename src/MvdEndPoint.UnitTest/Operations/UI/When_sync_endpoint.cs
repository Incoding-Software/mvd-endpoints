namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using CloudIn.Domain.Endpoint;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SyncEndpointCommand))]
    public class When_sync_endpoint
    {
        #region Establish value

        static MockMessage<SyncEndpointCommand, object> mockCommand;

        #endregion

        private static List<SyncEndpointCommand.GetEndpointsQuery.Response> responses;

        Establish establish = () =>
                              {
                                  SyncEndpointCommand command = Pleasure.Generator.Invent<SyncEndpointCommand>();

                                  var forRequest = Pleasure.ToList(Pleasure.Generator.Invent<SyncEndpointCommand.GetEndpointsQuery.Response.Property>(dsl => dsl.Tuning(r => r.Type, typeof(int?))));
                                  var forResponse = Pleasure.ToList(Pleasure.Generator.Invent<SyncEndpointCommand.GetEndpointsQuery.Response.Property>(dsl => dsl.Tuning(r => r.Type, typeof(DateTime?))));
                                  responses = Pleasure.ToList(Pleasure.Generator.Invent<SyncEndpointCommand.GetEndpointsQuery.Response>(dsl => dsl.Tuning(r => r.Responses, forResponse)
                                                                                                                                                  .Tuning(r => r.Requests, forRequest)));
                                  var getEndpointsQuery = Pleasure.Generator.Invent<SyncEndpointCommand.GetEndpointsQuery>();
                                  mockCommand = MockCommand<SyncEndpointCommand>
                                          .When(command)
                                          .StubQuery(getEndpointsQuery, responses);
                              };

        Because of = () => mockCommand.Execute();

        It should_be_save = () =>
                            {
                                mockCommand.ShouldBeSaveOrUpdate<Message>(endpoint =>
                                                                          {
                                                                              var response = responses[0];
                                                                              var allProps = new List<SyncEndpointCommand.GetEndpointsQuery.Response.Property>();
                                                                              allProps.AddRange(response.Requests);
                                                                              allProps.AddRange(response.Responses);
                                                                              Action<Message> byProperty = s => s.Properties.ShouldEqualWeakEach(allProps, (dsl, i) => dsl.ForwardToDefault(r => r.Description)
                                                                                                                                                                          .ForwardToDefault(r => r.Default)
                                                                                                                                                                          .ForwardToDefault(r => r.Message)
                                                                                                                                                                          .ForwardToValue(r => r.Type, i == 0 ? Message.Property.TypeOf.Request : Message.Property.TypeOf.Response)
                                                                                                                                                                          .ForwardToValue(r => r.PropertyType, allProps[i].Type.FullName)
                                                                                                                                                                          .ForwardToValue(r => r.GenericType, allProps[i].Type.GenericTypeArguments[0].FullName)
                                                                                                                                                                          .ForwardToDefault(r => r.GroupKey)
                                                                                                                                                                          .ForwardToDefault(r => r.IsRequired));
                                                                              endpoint.ShouldEqualWeak(response, dsl => dsl.ForwardToDefault(r => r.GroupKey)
                                                                                                                           .ForwardToAction(r => r.Properties, byProperty)
                                                                                                                           .ForwardToDefault(r => r.Description)
                                                                                                                           .ForwardToDefault(r => r.Jira)
                                                                                                                           .ForwardToDefault(r => r.Result)
                                                                                                                           .ForwardToValue(r => r.Name, response.Name));
                                                                          });
                            };
    }
}