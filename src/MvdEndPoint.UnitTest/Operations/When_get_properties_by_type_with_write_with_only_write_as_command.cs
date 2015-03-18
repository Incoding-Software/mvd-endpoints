namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetPropertiesByTypeQuery))]
    public class When_get_properties_by_type_with_write_with_only_write_as_command
    {
        #region Establish value

        static MockMessage<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>> mockQuery;

        #endregion

        Establish establish = () =>
                              {
                                  GetPropertiesByTypeQuery query = Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.IsCommand, true)
                                                                                                                                 .Tuning(r => r.Type, typeof(FakeCommand)));
                                  mockQuery = MockQuery<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>>
                                          .When(query);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldBeEmpty());

        public class FakeCommand
        {
            public string Display { get { return string.Empty; } }
        }
    }
}