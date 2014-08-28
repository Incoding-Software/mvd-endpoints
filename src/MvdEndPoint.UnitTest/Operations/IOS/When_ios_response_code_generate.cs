namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(IosResponseCodeGenerateQuery))]
    public class When_ios_response_code_generate
    {
        #region Fake classes

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

        #endregion

        #region Establish value

        static void Verify(FileOfIos fileOfIos, string fileName = null, bool withoutProperties = false)
        {
            var query = Pleasure.Generator.Invent<IosResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, fileOfIos)
                                                                                          .Tuning(r => r.Type, typeof(GetCustomerQuery)));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName ?? typeof(When_ios_response_code_generate).Name + "_" + fileOfIos.ToString().ToUpper()));

            var mockQuery = MockQuery<IosResponseCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)
                                                                                         .Tuning(r => r.Type, query.Type)), "GetCustomerResponse")
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Ios)
                                                                                             .Tuning(r => r.Type, typeof(GetCustomerQuery.Response))), withoutProperties ? new List<GetPropertiesByTypeQuery.Response>()
                                                                                                                                                               : new List<GetPropertiesByTypeQuery.Response>
                                                                                                                                                                     {
                                                                                                                                                                             new GetPropertiesByTypeQuery.Response { Name = "Title", Type = ConvertCSharpTypeToIosQuery.String },
                                                                                                                                                                             new GetPropertiesByTypeQuery.Response { Name = "Number", Type = ConvertCSharpTypeToIosQuery.Int },
                                                                                                                                                                             new GetPropertiesByTypeQuery.Response { Name = "Boolean", Type = ConvertCSharpTypeToIosQuery.Boolean, IsCanNull = false },
                                                                                                                                                                             new GetPropertiesByTypeQuery.Response { Name = "Type", Type = "MyEnum", IsEnum = true },
                                                                                                                                                                             new GetPropertiesByTypeQuery.Response { Name = "CreateDt", Type = ConvertCSharpTypeToIosQuery.Date, IsDateTime = true },
                                                                                                                                                                             new GetPropertiesByTypeQuery.Response { Name = "Ids", Type = ConvertCSharpTypeToIosQuery.String, IsArray = true },
                                                                                                                                                                     });
            mockQuery.Original.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        It should_be_h = () => Verify(FileOfIos.H);

        It should_be_m = () => Verify(FileOfIos.M);

        It should_be_without_properties_h = () => Verify(FileOfIos.H,
                                                         fileName: "When_ios_response_code_generate_as_get_without_properties_h",
                                                         withoutProperties: true); 
        
        It should_be_without_properties_m = () => Verify(FileOfIos.M,
                                                         fileName: "When_ios_response_code_generate_as_get_without_properties_m",
                                                         withoutProperties: true);
    }
}