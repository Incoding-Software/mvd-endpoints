namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AndroidResponseCodeGenerateQuery))]
    public class When_android_response_code_generate
    {
        It should_be_multiple = () => Run(expected: File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", "When_android_response_code_generate_as_array")),
                                          typeOfQuery: typeof(GetCustomersQuery),
                                          typeOfResponse: typeof(List<GetCustomersQuery.Response>));

        It should_be_single = () => Run(expected: File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", "When_android_response_code_generate")),
                                        typeOfQuery: typeof(GetCustomerQuery),
                                        typeOfResponse: typeof(GetCustomerQuery.Response));

        #region Establish value

        static void Run(string expected, Type typeOfQuery, Type typeOfResponse)
        {
            var query = Pleasure.Generator.Invent<AndroidResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeOfQuery));

            var properties = new List<GetPropertiesQuery.Response>
                             {
                                     new GetPropertiesQuery.Response { Name = "Title", Type = ConvertCSharpTypeToTargetQuery.ToJavaQuery.String },
                                     new GetPropertiesQuery.Response { Name = "Number", Type = ConvertCSharpTypeToTargetQuery.ToJavaQuery.Int },
                                     new GetPropertiesQuery.Response { Name = "Boolean", Type = ConvertCSharpTypeToTargetQuery.ToJavaQuery.Boolean },
                                     new GetPropertiesQuery.Response { Name = "Type", Type = "MyEnum", Attributes = GetPropertiesQuery.Response.OfAttributes.IsEnum },
                                     new GetPropertiesQuery.Response { Name = "CreateDt", Type = "java.util.Date", Attributes = GetPropertiesQuery.Response.OfAttributes.IsDateTime },
                                     new GetPropertiesQuery.Response { Name = "Ids", Type = "String", Attributes = GetPropertiesQuery.Response.OfAttributes.IsArray },
                                     new GetPropertiesQuery.Response() { Name = "Parent", Type = "InnerResponse", Attributes = GetPropertiesQuery.Response.OfAttributes.IsClass, Target = typeof(Nested1) },
                                     new GetPropertiesQuery.Response() { Name = "Items", Type = "InnerResponse", Attributes = GetPropertiesQuery.Response.OfAttributes.IsClass | GetPropertiesQuery.Response.OfAttributes.IsArray, Target = typeof(Nested2) },
                             };
            var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, typeOfQuery.Name)
                                                                                          .Tuning(r => r.IsCommand, false)
                                                                                          .Tuning(r => r.Namespace, "com.qabenchmarking.android.models"));
            var mockQuery = MockQuery<AndroidResponseCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery<AndroidNestedClassCodeGenerateQuery, string>(dsl => dsl.Tuning(r => r.Namespace, meta.Namespace)
                                                                                      .Tuning(r => r.Type, typeof(Nested1)), "nested class1")
                    .StubQuery<AndroidNestedClassCodeGenerateQuery, string>(dsl => dsl.Tuning(r => r.Namespace, meta.Namespace)
                                                                                      .Tuning(r => r.Type, typeof(Nested2)), "nested class2")
                    .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeOfQuery)),
                               meta)
                    .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, query.Type), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                  { { GetNameFromTypeQuery.ModeOf.Response, "GetCustomerResponse" } })
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Android)
                                                                                       .Tuning(r => r.IsCommand, false)
                                                                                       .Tuning(r => r.Type, typeOfResponse)), properties);

            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        #region Fake classes

        public class Nested1 { }

        public class Nested2 { }

        class GetCustomerQuery : QueryBase<GetCustomerQuery.Response>
        {
            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }

            #region Nested classes

            public class Response { }

            #endregion
        }

        class GetCustomersQuery : QueryBase<List<GetCustomersQuery.Response>>
        {
            protected override List<Response> ExecuteResult()
            {
                throw new NotImplementedException();
            }

            #region Nested classes

            public class Response { }

            #endregion
        }

        #endregion
    }
}