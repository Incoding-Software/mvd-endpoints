namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetPropertiesQuery))]
    public class When_get_properties_by_type_with_inheritance
    {
        #region Establish value

        static MockMessage<GetPropertiesQuery, List<GetPropertiesQuery.Response>> mockQuery;

        #endregion

        static string type;

        Establish establish = () =>
                              {
                                  GetPropertiesQuery query = Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.IsCommand, false)
                                                                                                                                     .Tuning(r => r.Type, typeof(IncBoolResponse)));

                                  type = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                                                         .Tuning(r => r.Type, typeof(bool))), type);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        new GetPropertiesQuery.Response()
                                                                                                        {
                                                                                                                Type = type,
                                                                                                                Attributes = GetPropertiesQuery.Response.OfAttributes.IsBool,
                                                                                                                Name = "Value",
                                                                                                                Target = typeof(bool)
                                                                                                        }
                                                                                                }));
    }
}