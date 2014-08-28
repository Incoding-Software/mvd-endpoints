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
    public class When_ios_request_code_generate_as_get
    {
        #region Fake classes

        class GetCustomerQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Verify(FileOfIos file, string fileName = null, bool withoutProperties = false)
        {
            var query = Pleasure.Generator.Invent<IosRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, file)
                                                                                         .Tuning(r => r.Type, typeof(GetCustomerQuery)));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName ?? typeof(When_ios_request_code_generate_as_get).Name + "_" + file.ToString().ToUpper()));

            var mockQuery = MockQuery<IosRequestCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))),
                               Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "GetCustomerQuery")))
                    .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)
                                                                                         .Tuning(r => r.Type, query.Type)), "GetCustomerRequest")
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Ios)
                                                                                             .Tuning(r => r.Type, typeof(GetCustomerQuery))), withoutProperties ? new List<GetPropertiesByTypeQuery.Response>()
                                                                                                                                                      : new List<GetPropertiesByTypeQuery.Response>
                                                                                                                                                            {
                                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Message")
                                                                                                                                                                                                                                           .Tuning(r => r.IsCanNull, true)
                                                                                                                                                                                                                                           .Tuning(r => r.IsArray, false)
                                                                                                                                                                                                                                           .Tuning(r => r.Type, "TheSameString")),
                                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Title")
                                                                                                                                                                                                                                           .Tuning(r => r.IsCanNull, false)
                                                                                                                                                                                                                                           .Tuning(r => r.IsArray, false)
                                                                                                                                                                                                                                           .Tuning(r => r.Type, "Number")),
                                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Ids")
                                                                                                                                                                                                                                           .Tuning(r => r.IsCanNull, false)
                                                                                                                                                                                                                                           .Tuning(r => r.IsArray, true)
                                                                                                                                                                                                                                           .Tuning(r => r.Type, "String"))
                                                                                                                                                            });
            mockQuery.Original.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        It should_be_h = () => Verify(FileOfIos.H);

        It should_be_m = () => Verify(FileOfIos.M);

        It should_be_without_properties_h = () => Verify(FileOfIos.H,
                                                         fileName: "When_ios_request_code_generate_as_get_without_properties_h",
                                                         withoutProperties: true);

        It should_be_without_properties_m = () => Verify(FileOfIos.M,
                                                         fileName: "When_ios_request_code_generate_as_get_without_properties_m",
                                                         withoutProperties: true);
    }
}