namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

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
            [UsedImplicitly]
            public string Value { get; set; }

            [UsedImplicitly]
            public int State { get; set; }

            [UsedImplicitly]
            public MyClassMyEnumForWhen_get_share_type_from_type MyClassMyEnumForWhenGetShareTypeFromType { get; set; }

            [UsedImplicitly]
            public MyEnumForWhen_get_share_type_from_type MyEnumForWhenGetShareTypeFromType { get; set; }

            [UsedImplicitly]
            public MyNesteadClass MyClassAsNestead { get; set; }

            public class MyNesteadClass { }
        }

        #region Establish value

        static MockMessage<GetShareTypeFromTypeQuery, List<Type>> mockQuery;

        static List<Type> expected;

        #endregion
    }
}