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
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Verify(FileOfIos file, Type type, string fileName, bool isArray, bool withoutProperties = false)
        {
            var query = Pleasure.Generator.Invent<IosRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, file)
                                                                                         .Tuning(r => r.Type, type));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));

            var mockQuery = MockQuery<IosRequestCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, type)), new IncBoolResponse(isArray))
                    .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)),
                               Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "GetCustomerQuery")))
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)
                                                                                         .Tuning(r => r.Type, query.Type)), "GetCustomerRequest")
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)
                                                                                         .Tuning(r => r.Type, query.Type)), "GetCustomerResponse")
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Ios)
                                                                                             .Tuning(r => r.Type, query.Type)), withoutProperties ? new List<GetPropertiesByTypeQuery.Response>()
                                                                                                                                        : new List<GetPropertiesByTypeQuery.Response>
                                                                                                                                              {
                                                                                                                                                      Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Message")
                                                                                                                                                                                                                             .Tuning(r => r.IsBool, false)
                                                                                                                                                                                                                             .Tuning(r => r.Type, "TheSameString")),
                                                                                                                                                      Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Title")
                                                                                                                                                                                                                             .Tuning(r => r.IsBool, false)
                                                                                                                                                                                                                             .Tuning(r => r.Type, "Number")),
                                                                                                                                                      Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Ids")
                                                                                                                                                                                                                             .Tuning(r => r.IsBool, false)
                                                                                                                                                                                                                             .Tuning(r => r.Type, "String")),
                                                                                                                                                      Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Is")
                                                                                                                                                                                                                             .Tuning(r => r.IsBool, true)
                                                                                                                                                                                                                             .Tuning(r => r.Type, "Bool"))
                                                                                                                                              });
            mockQuery.Original.Execute();
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