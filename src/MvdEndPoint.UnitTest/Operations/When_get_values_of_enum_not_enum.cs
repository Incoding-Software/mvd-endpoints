namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetValuesOfEnumQuery))]
    public class When_get_values_of_enum_not_enum
    {
        Establish establish = () =>
                              {
                                  GetValuesOfEnumQuery query = Pleasure.Generator.Invent<GetValuesOfEnumQuery>(dsl => dsl.Tuning(r => r.Type, typeof(When_get_values_of_enum_not_enum)));

                                  mockQuery = MockQuery<GetValuesOfEnumQuery, List<GetValuesOfEnumQuery.Response>>
                                          .When(query);
                              };

        Because of = () => { exception = Catch.Exception(() => mockQuery.Original.Execute()) as ArgumentException; };

        It should_be_result = () => exception.Message.ShouldEqual(@"When_get_values_of_enum_not_enum provided must be an Enum
Parameter name: Type");

        #region Establish value

        static MockMessage<GetValuesOfEnumQuery, List<GetValuesOfEnumQuery.Response>> mockQuery;

        private static Exception exception;

        #endregion
    }
}