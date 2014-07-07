namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
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

            [UsedImplicitly]
            public FakeEnum Enum { get; set; }

            [UsedImplicitly]
            public DateTime DateTime { get; set; }

            [UsedImplicitly, IgnoreDataMember]
            public int Ignore { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        enum FakeEnum
        { }

        static void Verify(Type type)
        {
            var query = Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, type));

            string dateTimeType = Pleasure.Generator.String();
            string enumType = Pleasure.Generator.String();
            string stringType = Pleasure.Generator.String();
            string intType = Pleasure.Generator.String();
            var mockQuery = MockQuery<GetPropertiesByTypeQuery, List<GetPropertiesByTypeQuery.Response>>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(string))), stringType)
                    .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum))), enumType)
                    .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(DateTime))), dateTimeType)
                    .StubQuery(Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, typeof(int))), intType);
            mockQuery.Original.Execute();
            mockQuery.ShouldBeIsResult(dictionary => dictionary.ShouldEqualWeakEach(new List<GetPropertiesByTypeQuery.Response>
                                                                                        {
                                                                                                new GetPropertiesByTypeQuery.Response { Name = "Name", Type = stringType, IsCanNull = true, IsEnum = false, IsDateTime = false },
                                                                                                new GetPropertiesByTypeQuery.Response { Name = "Sort", Type = intType, IsCanNull = false, IsEnum = false, IsDateTime = false },
                                                                                                new GetPropertiesByTypeQuery.Response { Name = "Enum", Type = enumType, IsCanNull = false, IsEnum = true, IsDateTime = false },
                                                                                                new GetPropertiesByTypeQuery.Response { Name = "DateTime", Type = dateTimeType, IsCanNull = true, IsEnum = false, IsDateTime = true }
                                                                                        }));
        }

        #endregion

        It should_be_by_type = () => Verify(typeof(FakeClass));

        It should_be_by_ienumerable_type = () => Verify(typeof(IList<FakeClass>));
    }
}