namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetPropertiesFromTypeQuery))]
    public class When_get_properties_by_type_with_write_with_only_write_as_command
    {
        #region Establish value

        static MockMessage<GetPropertiesFromTypeQuery, List<GetPropertiesFromTypeQuery.Response>> mockQuery;

        #endregion

        Establish establish = () =>
                              {
                                  GetPropertiesFromTypeQuery query = Pleasure.Generator.Invent<GetPropertiesFromTypeQuery>(dsl => dsl.Tuning(r => r.IsCommand, true)
                                                                                                                                 .Tuning(r => r.Type, typeof(FakeCommand)));
                                  mockQuery = MockQuery<GetPropertiesFromTypeQuery, List<GetPropertiesFromTypeQuery.Response>>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldBeEmpty());

        public class FakeCommand
        {
            public string Display { get { return string.Empty; } }
        }
    }
}