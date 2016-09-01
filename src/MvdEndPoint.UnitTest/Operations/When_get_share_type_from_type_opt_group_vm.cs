namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetShareTypeFromTypeQuery))]
    public class When_get_share_type_from_type_opt_group_vm
    {
        Establish establish = () =>
                              {
                                  GetShareTypeFromTypeQuery query = Pleasure.Generator.Invent<GetShareTypeFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(OptGroupVm)));
                                  expected = new List<Type>()
                                             { typeof(KeyValueVm), typeof(WPSampleCodeGenerateQuery), typeof(WPGenerateQueryQuery) };

                                  mockQuery = MockQuery<GetShareTypeFromTypeQuery, List<Type>>
                                          .When(query)
                                          .StubQuery<GetShareTypeFromTypeQuery, List<Type>>(dsl => dsl.Tuning(s => s.Type, typeof(KeyValueVm)), new List<Type>() { expected[1], expected[2] });
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeak(expected));

        #region Establish value

        static MockMessage<GetShareTypeFromTypeQuery, List<Type>> mockQuery;

        static List<Type> expected;

        #endregion
    }
}