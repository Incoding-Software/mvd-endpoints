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

    [Subject(typeof(AndroidRequestCodeGenerateQuery))]
    public class When_ios_request_code_generate
    {
        #region Fake classes

        class GetCustomerQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        class AddCustomerCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Verify(FileOfIos file, Type type, string fileName, bool isArray, bool withoutProperties = false, bool isImage = false)
        {
            var query = Pleasure.Generator.Invent<IosRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, file)
                                                                                         .Tuning(r => r.Type, type));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));

            var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.IsCommand, type.Name.EndsWith("Command"))
                                                                                          .Tuning(r => r.ResponseAsArray, isArray)
                                                                                          .Tuning(r => r.ResponseAsImage, isImage)
                                                                                          .Tuning(r => r.Name, "GetCustomerQuery"));
            var mockQuery = MockQuery<IosRequestCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), meta)
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                            {
                                                                                                                                    { GetNameFromTypeQuery.ModeOf.Request, "GetCustomerRequest" }, 
                                                                                                                                    { GetNameFromTypeQuery.ModeOf.Response, "GetCustomerResponse" }, 
                                                                                                                            })
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Ios)
                                                                                               .Tuning(r => r.IsCommand, meta.IsCommand)
                                                                                               .Tuning(r => r.Type, query.Type)), withoutProperties ? new List<GetPropertiesQuery.Response>()
                                                                                                                                          : new List<GetPropertiesQuery.Response>
                                                                                                                                            {
                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Message")
                                                                                                                                                                                                                             .Tuning(r => r.Type, "TheSameString")), 
                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Title")
                                                                                                                                                                                                                             .Tuning(r => r.Type, "Number")), 
                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Ids")
                                                                                                                                                                                                                             .Tuning(r => r.Type, "String")), 
                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Is")
                                                                                                                                                                                                                             .Tuning(r => r.Type, ConvertCSharpTypeToIosQuery.Boolean))
                                                                                                                                            });
            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        It should_be_h = () => Verify(file: FileOfIos.H, 
                                      type: typeof(AddCustomerCommand), 
                                      isArray: false, 
                                      fileName: "When_ios_request_code_generate_h");

        It should_be_m = () => Verify(file: FileOfIos.M, 
                                      type: typeof(AddCustomerCommand), 
                                      isArray: false, 
                                      fileName: "When_ios_request_code_generate_m");

        It should_be_query_h = () => Verify(file: FileOfIos.H, 
                                            type: typeof(GetCustomerQuery), 
                                            isArray: false, 
                                            fileName: "When_ios_request_code_generate_as_query_h");

        It should_be_query_m = () => Verify(file: FileOfIos.M, 
                                            type: typeof(GetCustomerQuery), 
                                            isArray: false, 
                                            fileName: "When_ios_request_code_generate_as_query_m");

        It should_be_query_with_array_h = () => Verify(file: FileOfIos.H, 
                                                       type: typeof(GetCustomerQuery), 
                                                       isArray: true, 
                                                       fileName: "When_ios_request_code_generate_as_query_with_array_h");

        It should_be_query_with_array_m = () => Verify(file: FileOfIos.M, 
                                                       type: typeof(GetCustomerQuery), 
                                                       isArray: true, 
                                                       fileName: "When_ios_request_code_generate_as_query_with_array_m");

        It should_be_without_properties_h = () => Verify(file: FileOfIos.H, 
                                                         type: typeof(GetCustomerQuery), 
                                                         isArray: false, 
                                                         fileName: "When_ios_request_code_generate_as_query_without_properties_h", 
                                                         withoutProperties: true);

        It should_be_without_properties_m = () => Verify(file: FileOfIos.M, 
                                                         type: typeof(GetCustomerQuery), 
                                                         isArray: false, 
                                                         fileName: "When_ios_request_code_generate_as_query_without_properties_m", 
                                                         withoutProperties: true);
    }
}