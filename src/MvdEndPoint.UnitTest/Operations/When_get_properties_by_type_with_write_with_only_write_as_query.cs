namespace MvdEndPoint.UnitTest
{
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    [Subject(typeof(GetPropertiesByTypeQuery))]
    public class When_get_properties_by_type_with_write_with_only_write_as_query
    {
        #region Establish value

        static MockMessage<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>> mockQuery;

        #endregion

        static string type;

        Establish establish = () =>
                              {
                                  GetPropertiesByTypeQuery query = Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.IsCommand, false)
                                                                                                                                 .Tuning(r => r.Type, typeof(FakeCommand)));

                                  type = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                                                         .Tuning(r => r.Type, typeof(string))), type);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(new[]
                                                                                                {
                                                                                                        new GetPropertiesByTypeQuery.Response()
                                                                                                        {
                                                                                                                IsArray = false,
                                                                                                                IsBool = false,
                                                                                                                IsEnum = false,
                                                                                                                IsCanNull = true,
                                                                                                                IsDateTime = false,
                                                                                                                Type = type
                                                                                                        }
                                                                                                }));

        public class FakeCommand
        {
            public string Display { get { return string.Empty; } }
        }
    }
}