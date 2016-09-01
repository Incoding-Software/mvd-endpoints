namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(WPSampleCodeGenerateQuery))]
    public class When_wp_sample_code_generate
    {
        #region Establish value

        static MockMessage<WPSampleCodeGenerateQuery, string> mockQuery;

        #endregion

        private static string expected;

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<WPSampleCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Instance, Pleasure.Generator.Invent<GetCustomerQuery>()));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample_Code_Generate", typeof(When_wp_sample_code_generate).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "Incoding")
                                                                                                                .Tuning(r => r.Name, "GetCustomerQuery"));

                                  mockQuery = MockQuery<WPSampleCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))), meta)
                                          .StubQuery(Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Language, Language.Csharp)
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

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        public class GetCustomerQuery { }
    }
}