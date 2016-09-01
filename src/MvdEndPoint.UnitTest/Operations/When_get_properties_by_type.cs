namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(GetPropertiesQuery))]
    public class When_get_properties_by_type
    {
        It should_be_by_ienumerable_type_android = () => Verify(typeof(IList<FakeClass>));

        It should_be_by_type = () => Verify(typeof(FakeClass));

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

            [UsedImplicitly]
            public DateTime? DateTimeAsNullable { get; set; }

            [UsedImplicitly, IgnoreDataMember]
            public int Ignore { get; set; }

            [UsedImplicitly]
            public string[] Array { get; set; }

            [UsedImplicitly]
            public bool Bool { get; set; }

            [UsedImplicitly]
            public FakeClass CustomClass { get; set; }

            [UsedImplicitly]
            public FakeClass[] CustomClassOfArray { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        enum FakeEnum
        { }

        static void Verify(Type type)
        {
            var query = Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Type, type));

            string dateTimeType = Pleasure.Generator.String();
            string enumType = Pleasure.Generator.String();
            string stringType = Pleasure.Generator.String();
            string intType = Pleasure.Generator.String();
            string boolType = Pleasure.Generator.String();

            var mockQuery = MockQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>
                    .When(query)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(FakeClass)), typeof(FakeClass).Name)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(FakeClass[])), typeof(FakeClass).Name)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(string)), stringType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(FakeEnum)), enumType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(DateTime)), dateTimeType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(DateTime?)), dateTimeType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(int)), intType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Language, query.Language)
                                                                                 .Tuning(r => r.Type, typeof(bool)), boolType);

            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(dictionary => dictionary.ShouldEqualWeakEach(new List<GetPropertiesQuery.Response>
                                                                                    {
                                                                                            new GetPropertiesQuery.Response { Name = "Name", Type = stringType, Attributes = GetPropertiesQuery.Response.OfAttributes.IsCanNull, Target = typeof(string) },
                                                                                            new GetPropertiesQuery.Response { Name = "Sort", Type = intType, Target = typeof(int) },
                                                                                            new GetPropertiesQuery.Response { Name = "Enum", Type = enumType, Attributes = GetPropertiesQuery.Response.OfAttributes.IsEnum, Target = typeof(FakeEnum) },
                                                                                            new GetPropertiesQuery.Response { Name = "DateTime", Type = dateTimeType, Attributes = GetPropertiesQuery.Response.OfAttributes.IsDateTime | GetPropertiesQuery.Response.OfAttributes.IsCanNull, Target = typeof(DateTime) },
                                                                                            new GetPropertiesQuery.Response { Name = "DateTimeAsNullable", Type = dateTimeType, Attributes = GetPropertiesQuery.Response.OfAttributes.IsDateTime | GetPropertiesQuery.Response.OfAttributes.IsCanNull, Target = typeof(DateTime?) },
                                                                                            new GetPropertiesQuery.Response { Name = "Array", Type = stringType, Attributes = GetPropertiesQuery.Response.OfAttributes.IsCanNull | GetPropertiesQuery.Response.OfAttributes.IsArray, Target = typeof(string) },
                                                                                            new GetPropertiesQuery.Response { Name = "Bool", Type = boolType, Attributes = GetPropertiesQuery.Response.OfAttributes.IsBool, Target = typeof(bool) },
                                                                                            new GetPropertiesQuery.Response { Name = "CustomClass", Type = typeof(FakeClass).Name, Attributes = GetPropertiesQuery.Response.OfAttributes.IsCanNull | GetPropertiesQuery.Response.OfAttributes.IsClass, Target = typeof(FakeClass) },
                                                                                            new GetPropertiesQuery.Response { Name = "CustomClassOfArray", Type = typeof(FakeClass).Name, Attributes = GetPropertiesQuery.Response.OfAttributes.IsCanNull | GetPropertiesQuery.Response.OfAttributes.IsClass | GetPropertiesQuery.Response.OfAttributes.IsArray, Target = typeof(FakeClass) }
                                                                                    }));
        }

        #endregion
    }
}