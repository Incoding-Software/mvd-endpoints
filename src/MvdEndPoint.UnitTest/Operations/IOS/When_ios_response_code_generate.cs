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

    [Subject(typeof(IosResponseCodeGenerateQuery))]
    public class When_ios_response_code_generate
    {
        It should_be_as_array_h = () => Verify(FileOfIos.H, fileName: "When_ios_response_code_generate_as_array_h", isArray: true);

        It should_be_as_array_m = () => Verify(FileOfIos.M, fileName: "When_ios_response_code_generate_as_array_m", isArray: true);

        It should_be_command_h = () => VerifyCommand(FileOfIos.H, fileName: "When_ios_response_code_generate_command_h");

        It should_be_command_m = () => VerifyCommand(FileOfIos.M, fileName: "When_ios_response_code_generate_command_m");

        It should_be_h = () => Verify(FileOfIos.H, fileName: "When_ios_response_code_generate_h");

        It should_be_m = () => Verify(FileOfIos.M, fileName: "When_ios_response_code_generate_m");

        #region Fake classes

        class AddCustomerCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

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

        #endregion

        #region Establish value

        static void Verify(FileOfIos fileOfIos, string fileName, bool withoutProperties = false, bool isArray = false)
        {
            var query = Pleasure.Generator.Invent<IosResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, fileOfIos)
                                                                                          .Tuning(r => r.Type, typeof(GetCustomerQuery)));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", fileName));

            var mockQuery = MockQuery<IosResponseCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), isArray)
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                            {
                                                                                                                                    { GetNameFromTypeQuery.ModeOf.Response, "GetCustomerResponse" },
                                                                                                                            })
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Language, Language.ObjectiveC)
                                                                                       .Tuning(r => r.Type, typeof(GetCustomerQuery.Response))), withoutProperties ? new List<GetPropertiesQuery.Response>()
                                                                                                                                                         : new List<GetPropertiesQuery.Response>
                                                                                                                                                           {
                                                                                                                                                                   new GetPropertiesQuery.Response { Name = "Title", Type = ConvertCSharpTypeToTargetQuery.ToIosQuery.String },
                                                                                                                                                                   new GetPropertiesQuery.Response { Name = "Number", Type = ConvertCSharpTypeToTargetQuery.ToIosQuery.Int },
                                                                                                                                                                   new GetPropertiesQuery.Response { Name = "Boolean", Type = ConvertCSharpTypeToTargetQuery.ToIosQuery.Boolean },
                                                                                                                                                                   new GetPropertiesQuery.Response { Name = "Type", Type = "MyEnum", Attributes = GetPropertiesQuery.Response.OfAttributes.IsEnum },
                                                                                                                                                                   new GetPropertiesQuery.Response { Name = "CreateDt", Type = ConvertCSharpTypeToTargetQuery.ToIosQuery.Date, Attributes = GetPropertiesQuery.Response.OfAttributes.IsDateTime },
                                                                                                                                                                   new GetPropertiesQuery.Response { Name = "Ids", Type = ConvertCSharpTypeToTargetQuery.ToIosQuery.String, Attributes = GetPropertiesQuery.Response.OfAttributes.IsArray },
                                                                                                                                                           });
            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        static void VerifyCommand(FileOfIos fileOfIos, string fileName)
        {
            var query = Pleasure.Generator.Invent<IosResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, fileOfIos)
                                                                                          .Tuning(r => r.Type, typeof(AddCustomerCommand)));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", fileName));

            var mockQuery = MockQuery<IosResponseCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery<IsCommandTypeQuery, bool>(true)
                    .StubQuery(Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), false)
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                            {
                                                                                                                                    { GetNameFromTypeQuery.ModeOf.Response, "GetCustomerResponse" },
                                                                                                                            });

            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion
    }
}