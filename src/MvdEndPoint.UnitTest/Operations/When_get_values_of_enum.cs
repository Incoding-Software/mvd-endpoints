namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.ComponentModel;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetValuesOfEnumQuery))]
    public class When_get_values_of_enum
    {
        public enum FakeEnum
        {
            [Description("Value 1")]
            Value = 1,

            [Description("NextValue 1")]
            NextValue = 5,

            NextWithoutDescription = 7
        }

        Establish establish = () =>
                              {
                                  GetValuesOfEnumQuery query = Pleasure.Generator.Invent<GetValuesOfEnumQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum)));
                                  expected = new[]
                                             {
                                                     new GetValuesOfEnumQuery.Response()
                                                     {
                                                             Display = "Value 1",
                                                             AsString = "Value",
                                                             AsInt = "1"
                                                     },
                                                     new GetValuesOfEnumQuery.Response()
                                                     {
                                                             Display = "NextValue 1",
                                                             AsString = "NextValue",
                                                             AsInt = "5"
                                                     },
                                                     new GetValuesOfEnumQuery.Response()
                                                     {
                                                             Display = "",
                                                             AsString = "NextWithoutDescription",
                                                             AsInt = "7"
                                                     }
                                             };

                                  mockQuery = MockQuery<GetValuesOfEnumQuery, List<GetValuesOfEnumQuery.Response>>
                                          .When(query);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected));

        #region Establish value

        static MockMessage<GetValuesOfEnumQuery, List<GetValuesOfEnumQuery.Response>> mockQuery;

        static GetValuesOfEnumQuery.Response[] expected;

        #endregion
    }
}