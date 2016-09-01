namespace MvdEndPoint.UnitTest
{
    using System;
    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(GetValuesOfEnumQuery))]
    public class When_get_values_of_enum_is_null
    {
        Establish establish = () =>
                              {
                                  GetValuesOfEnumQuery query = Pleasure.Generator.Invent<GetValuesOfEnumQuery>(dsl => dsl.Tuning(r => r.Type, null));

                                  mockQuery = MockQuery<GetValuesOfEnumQuery, List<GetValuesOfEnumQuery.Response>>
                                          .When(query);
                              };

        Because of = () => { exception = Catch.Exception(() => mockQuery.Original.Execute()) as ArgumentException; };

        It should_be_result = () => exception.Message.ShouldEqual(@"Argument Type can't be null
Parameter name: Type");

        #region Establish value

        static MockMessage<GetValuesOfEnumQuery, List<GetValuesOfEnumQuery.Response>> mockQuery;

        private static Exception exception;

        #endregion
    }
}