namespace MvdEndPoint.UnitTest
{
    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    [Subject(typeof(GetPropertiesQuery))]
    public class When_get_properties_by_type_with_message_base
    {
        #region Establish value

        static MockMessage<GetPropertiesQuery, List<GetPropertiesQuery.Response>> mockQuery;

        #endregion

        static string type;

        Establish establish = () =>
                              {
                                  GetPropertiesQuery query = Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.IsCommand, true)
                                                                                                                                 .Tuning(r => r.Type, typeof(FakeCommand)));

                                  type = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldBeEmpty());

        public class FakeCommand : MessageBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}