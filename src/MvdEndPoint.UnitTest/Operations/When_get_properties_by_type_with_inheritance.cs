namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetPropertiesByTypeQuery))]
    public class When_get_properties_by_type_with_inheritance
    {
        #region Establish value

        static MockMessage<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>> mockQuery;

        #endregion

        static string type;

        Establish establish = () =>
                              {
                                  GetPropertiesByTypeQuery query = Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.IsCommand, false)
                                                                                                                                 .Tuning(r => r.Type, typeof(IncBoolResponse)));

                                  type = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                                                         .Tuning(r => r.Type, typeof(bool))), type);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        new GetPropertiesByTypeQuery.Response()
                                                                                                        {
                                                                                                                Type = type,
                                                                                                                IsBool = true,
                                                                                                                IsArray = false,
                                                                                                                IsCanNull = false,
                                                                                                                IsDateTime = false,
                                                                                                                IsEnum = false,
                                                                                                                Name = "Value"
                                                                                                        }
                                                                                                }));
    }
}