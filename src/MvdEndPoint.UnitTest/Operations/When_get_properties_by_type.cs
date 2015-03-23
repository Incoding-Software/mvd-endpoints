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

    [Subject(typeof(GetPropertiesFromTypeQuery))]
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
            var query = Pleasure.Generator.Invent<GetPropertiesFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, type));

            string dateTimeType = Pleasure.Generator.String();
            string enumType = Pleasure.Generator.String();
            string stringType = Pleasure.Generator.String();
            string intType = Pleasure.Generator.String();
            string boolType = Pleasure.Generator.String();

            var mockQuery = MockQuery<GetPropertiesFromTypeQuery, List<GetPropertiesFromTypeQuery.Response>>
                    .When(query)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                 .Tuning(r => r.Type, typeof(FakeClass)), typeof(FakeClass).Name)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                 .Tuning(r => r.Type, typeof(FakeClass[])), typeof(FakeClass).Name)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                 .Tuning(r => r.Type, typeof(string)), stringType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                 .Tuning(r => r.Type, typeof(FakeEnum)), enumType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                 .Tuning(r => r.Type, typeof(DateTime)), dateTimeType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                 .Tuning(r => r.Type, typeof(int)), intType)
                    .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(r => r.Device, query.Device)
                                                                                 .Tuning(r => r.Type, typeof(bool)), boolType);

            mockQuery.Original.Execute();
            mockQuery.ShouldBeIsResult(dictionary => dictionary.ShouldEqualWeakEach(new List<GetPropertiesFromTypeQuery.Response>
                                                                                    {
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "Name", Type = stringType, Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsCanNull, Target = typeof(string) },
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "Sort", Type = intType, Target = typeof(int) },
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "Enum", Type = enumType, Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsEnum, Target = typeof(FakeEnum) },
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "DateTime", Type = dateTimeType, Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsDateTime | GetPropertiesFromTypeQuery.Response.OfAttributes.IsCanNull, Target = typeof(DateTime) },
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "Array", Type = stringType, Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsCanNull | GetPropertiesFromTypeQuery.Response.OfAttributes.IsArray, Target = typeof(string) },
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "Bool", Type = boolType, Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsBool, Target = typeof(bool) },
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "CustomClass", Type = typeof(FakeClass).Name, Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsCanNull | GetPropertiesFromTypeQuery.Response.OfAttributes.IsClass, Target = typeof(FakeClass) },
                                                                                            new GetPropertiesFromTypeQuery.Response { Name = "CustomClassOfArray", Type = typeof(FakeClass).Name, Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsCanNull | GetPropertiesFromTypeQuery.Response.OfAttributes.IsClass | GetPropertiesFromTypeQuery.Response.OfAttributes.IsArray, Target = typeof(FakeClass) }
                                                                                    }));
        }

        #endregion
    }
}