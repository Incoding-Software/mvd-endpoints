namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetPropertiesFromTypeQuery))]
    public class When_get_properties_by_type_with_inheritance
    {
        #region Establish value

        static MockMessage<GetPropertiesFromTypeQuery, List<GetPropertiesFromTypeQuery.Response>> mockQuery;

        #endregion

        static string type;

        Establish establish = () =>
                              {
                                  GetPropertiesFromTypeQuery query = Pleasure.Generator.Invent<GetPropertiesFromTypeQuery>(dsl => dsl.Tuning(r => r.IsCommand, false)
                                                                                                                                     .Tuning(r => r.Type, typeof(IncBoolResponse)));

                                  type = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesFromTypeQuery, List<GetPropertiesFromTypeQuery.Response>>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                                                         .Tuning(r => r.Type, typeof(bool))), type);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        new GetPropertiesFromTypeQuery.Response()
                                                                                                        {
                                                                                                                Type = type,
                                                                                                                Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsBool,
                                                                                                                Name = "Value",
                                                                                                                Target = typeof(bool)
                                                                                                        }
                                                                                                }));
    }
}