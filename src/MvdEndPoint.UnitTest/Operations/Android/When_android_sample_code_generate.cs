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

    [Subject(typeof(AndroidSampleCodeGenerateQuery))]
    public class When_android_sample_code_generate
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidSampleCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Instance, Pleasure.Generator.Invent<GetCustomerQuery>()));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", typeof(When_android_sample_code_generate).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "Incoding")
                                                                                                                .Empty(r => r.ResponseAsArray)
                                                                                                                .Tuning(r => r.Name, "GetCustomerQuery"));

                                  mockQuery = MockQuery<AndroidSampleCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))), meta)
                                          .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery)), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                                                      {
                                                                                                                                                                                              { GetNameFromTypeQuery.ModeOf.Request, "GetCustomerRequest" },
                                                                                                                                                                                              { GetNameFromTypeQuery.ModeOf.Listener, "IGetCustomerListene" },
                                                                                                                                                                                              { GetNameFromTypeQuery.ModeOf.Response, "GetCustomerResponse" }
                                                                                                                                                                                      })
                                          .StubQuery(Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Android)
                                                                                                             .Tuning(r => r.IsCommand, meta.IsCommand)
                                                                                                             .Tuning(r => r.Type, typeof(GetCustomerQuery))), new List<GetPropertiesQuery.Response>
                                                                                                                                                              {
                                                                                                                                                                      Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Message")
                                                                                                                                                                                                                                       .Tuning(r => r.Attributes, GetPropertiesQuery.Response.OfAttributes.IsCanNull)
                                                                                                                                                                                                                                       .Tuning(r => r.Type, "TheSameString")),
                                                                                                                                                                      Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Title")
                                                                                                                                                                                                                                       .Empty(r => r.Attributes)
                                                                                                                                                                                                                                       .Tuning(r => r.Type, "Number")),
                                                                                                                                                                      Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Ids")
                                                                                                                                                                                                                                       .Tuning(r => r.Attributes, GetPropertiesQuery.Response.OfAttributes.IsArray)
                                                                                                                                                                                                                                       .Tuning(r => r.Type, "String"))
                                                                                                                                                              });
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

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

        static MockMessage<AndroidSampleCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}