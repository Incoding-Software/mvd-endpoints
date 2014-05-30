namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetPropertiesByTypeQuery))]
    public class When_get_properties_by_type
    {
        #region Fake classes

        class FakeClass
        {
            #region Properties

            [UsedImplicitly]
            public string Name { get; set; }

            [UsedImplicitly]
            public int Sort { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static void Verify(Type type)
        {
            var query = Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, type));

            string stringType = Pleasure.Generator.String();
            string intType = Pleasure.Generator.String();
            var mockQuery = MockQuery<GetPropertiesByTypeQuery, Dictionary<string, string>>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(string))), stringType)
                    .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(int))), intType);
            mockQuery.Original.Execute();
            mockQuery.ShouldBeIsResult(dictionary => dictionary.ShouldEqualWeakEach(new Dictionary<string, string>
                                                                                        {
                                                                                                { "Name", stringType },
                                                                                                { "Sort", intType },
                                                                                        }));
        }

        #endregion

        It should_be_by_type = () => Verify(typeof(FakeClass));

        It should_be_by_ienumerable_type = () => Verify(typeof(IList<FakeClass>));
    }
}