namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    public enum MyEnumForWhen_get_share_type_from_type
    { }

    public class MyClassMyEnumForWhen_get_share_type_from_type { }

    [Subject(typeof(GetShareTypeFromTypeQuery))]
    public class When_get_share_type_from_type
    {
        Establish establish = () =>
                              {
                                  GetShareTypeFromTypeQuery query = Pleasure.Generator.Invent<GetShareTypeFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeClass)));
                                  expected = new List<Type>()
                                             {
                                                     typeof(MyClassMyEnumForWhen_get_share_type_from_type),
                                                     typeof(MyEnumForWhen_get_share_type_from_type)
                                             };

                                  mockQuery = MockQuery<GetShareTypeFromTypeQuery, List<Type>>
                                          .When(query);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        public class FakeClass
        {
            public string Value { get; set; }

            public int State { get; set; }

            public MyClassMyEnumForWhen_get_share_type_from_type MyClassMyEnumForWhenGetShareTypeFromType { get; set; }

            public MyEnumForWhen_get_share_type_from_type MyEnumForWhenGetShareTypeFromType { get; set; }

            public MyNesteadClass MyClassAsNestead { get; set; }

            public class MyNesteadClass { }
        }

        #region Establish value

        static MockMessage<GetShareTypeFromTypeQuery, List<Type>> mockQuery;

        static List<Type> expected;

        #endregion
    }
}