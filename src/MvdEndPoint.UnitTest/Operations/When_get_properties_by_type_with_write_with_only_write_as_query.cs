namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetPropertiesFromTypeQuery))]
    public class When_get_properties_by_type_with_write_with_only_write_as_query
    {
        #region Establish value

        static MockMessage<GetPropertiesFromTypeQuery, List<GetPropertiesFromTypeQuery.Response>> mockQuery;

        #endregion

        static string type;

        Establish establish = () =>
                              {
                                  GetPropertiesFromTypeQuery query = Pleasure.Generator.Invent<GetPropertiesFromTypeQuery>(dsl => dsl.Tuning(r => r.IsCommand, false)
                                                                                                                                 .Tuning(r => r.Type, typeof(FakeCommand)));

                                  type = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesFromTypeQuery, List<GetPropertiesFromTypeQuery.Response>>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                                                         .Tuning(r => r.Type, typeof(string))), type);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        new GetPropertiesFromTypeQuery.Response()
                                                                                                        {
                                                                                                                Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsCanNull,
                                                                                                                Name = "Display",
                                                                                                                Type = type,
                                                                                                                Target = typeof(string)
                                                                                                        }
                                                                                                }));

        public class FakeCommand
        {
            public string Display { get { return string.Empty; } }
        }
    }
}