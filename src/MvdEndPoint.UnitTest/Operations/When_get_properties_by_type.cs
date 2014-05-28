namespace MvdEndPoint.UnitTest
{
    #region << Using >>

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

        static MockMessage<GetPropertiesByTypeQuery, Dictionary<string, string>> mockQuery;

        static string stringType;

        static string intType;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeClass)));

                                      stringType = Pleasure.Generator.String();
                                      intType = Pleasure.Generator.String();
                                      mockQuery = MockQuery<GetPropertiesByTypeQuery, Dictionary<string, string>>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(string))), stringType)
                                              .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(int))), intType);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(dictionary => dictionary.ShouldEqualWeakEach(new Dictionary<string, string>
                                                                                                                {
                                                                                                                        { "Name", stringType },
                                                                                                                        { "Sort", intType },
                                                                                                                }));
    }
}