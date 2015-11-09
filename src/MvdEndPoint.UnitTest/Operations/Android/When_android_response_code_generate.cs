namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;
    using MvdEndPoint.Domain.Operations;

    #endregion

    [Subject(typeof(AndroidResponseCodeGenerateQuery))]
    public class When_android_response_code_generate
    {
        #region Fake classes

        public class Nested1 { }

        public class Nested2 { }

        class GetCustomerQuery : QueryBase<GetCustomerQuery.Response>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        class GetCustomersQuery : QueryBase<List<GetCustomersQuery.Response>>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override List<Response> ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Run(string expected, Type typeOfQuery, Type typeOfResponse)
        {
            var query = Pleasure.Generator.Invent<AndroidResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeOfQuery));

            var properties = new List<GetPropertiesFromTypeQuery.Response>
                             {
                                     new GetPropertiesFromTypeQuery.Response { Name = "Title", Type = ConvertCSharpTypeToJavaQuery.String }, 
                                     new GetPropertiesFromTypeQuery.Response { Name = "Number", Type = ConvertCSharpTypeToJavaQuery.Int }, 
                                     new GetPropertiesFromTypeQuery.Response { Name = "Boolean", Type = ConvertCSharpTypeToJavaQuery.Boolean }, 
                                     new GetPropertiesFromTypeQuery.Response { Name = "Type", Type = "MyEnum", Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsEnum }, 
                                     new GetPropertiesFromTypeQuery.Response { Name = "CreateDt", Type = "java.util.Date", Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsDateTime }, 
                                     new GetPropertiesFromTypeQuery.Response { Name = "Ids", Type = "String", Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsArray }, 
                                     new GetPropertiesFromTypeQuery.Response() { Name = "Parent", Type = "InnerResponse", Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsClass, Target = typeof(Nested1) }, 
                                     new GetPropertiesFromTypeQuery.Response() { Name = "Items", Type = "InnerResponse", Attributes = GetPropertiesFromTypeQuery.Response.OfAttributes.IsClass | GetPropertiesFromTypeQuery.Response.OfAttributes.IsArray, Target = typeof(Nested2) }, 
                             };
            var mockQuery = MockQuery<AndroidResponseCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery<AndroidNestedClassCodeGenerateQuery, string>(dsl => dsl.Tuning(r => r.Type, typeof(Nested1)), "nested class1")
                    .StubQuery<AndroidNestedClassCodeGenerateQuery, string>(dsl => dsl.Tuning(r => r.Type, typeof(Nested2)), "nested class2")
                    .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeOfQuery)), 
                               Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, typeOfQuery.Name)
                                                                                                  .Tuning(r => r.Package, "com.qabenchmarking.android.models.{0}".F(typeOfQuery.Name))
                                                                                                  .Tuning(r => r.Namespace, "com.qabenchmarking.android.models")))
                    .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, query.Type), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                  { { GetNameFromTypeQuery.ModeOf.Response, "GetCustomerResponse" } })
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesFromTypeQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Android)
                                                                                               .Tuning(r => r.IsCommand, false)
                                                                                               .Tuning(r => r.Type, typeOfResponse)), properties);

            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        It should_be_multiple = () => Run(expected: File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "When_android_response_code_generate_as_array")), 
                                          typeOfQuery: typeof(GetCustomersQuery), 
                                          typeOfResponse: typeof(List<GetCustomersQuery.Response>));

        It should_be_single = () => Run(expected: File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "When_android_response_code_generate")), 
                                        typeOfQuery: typeof(GetCustomerQuery), 
                                        typeOfResponse: typeof(GetCustomerQuery.Response));
    }
}