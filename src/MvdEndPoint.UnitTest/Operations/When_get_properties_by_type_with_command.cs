﻿namespace MvdEndPoint.UnitTest
{
    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    [Subject(typeof(GetPropertiesByTypeQuery))]
    public class When_get_properties_by_type_with_message_base
    {
        #region Establish value

        static MockMessage<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>> mockQuery;

        #endregion

        static string type;

        Establish establish = () =>
                              {
                                  GetPropertiesByTypeQuery query = Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.IsCommand, true)
                                                                                                                                 .Tuning(r => r.Type, typeof(FakeCommand)));

                                  type = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>>
                                          .When(query);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldBeEmpty());

        public class FakeCommand : MessageBase<object>
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}