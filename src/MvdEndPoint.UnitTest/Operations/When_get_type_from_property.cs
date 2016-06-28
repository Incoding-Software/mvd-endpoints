namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetTypeFromPropertyQuery))]
    public class When_get_type_from_property
    {
        It should_be_array = () => Run(typeof(byte).FullName, typeof(byte[]));

        It should_be_string = () => Run(typeof(String).FullName, typeof(String));

        public static void Run(string expected, Type result)
        {
            GetTypeFromPropertyQuery query = Pleasure.Generator.Invent<GetTypeFromPropertyQuery>(dsl => dsl.GenerateTo(r => r.Property, factoryDsl => factoryDsl.Empty(s => s.GenericType)
                                                                                                                                                                .Tuning(s => s.PropertyType, result.FullName)));

            var mockQuery = MockQuery<GetTypeFromPropertyQuery, Type>
                    .When(query)
                    .StubQuery<CreateByTypeQuery.FindTypeByName, Type>(dsl => dsl.Tuning(r => r.Type, expected), result);
            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(result);
        }
    }
}