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

    [Subject(typeof(RequestCodeGenerateQuery))]
    public class When_request_code_generate_as_post
    {
        #region Fake classes

        class AddCustomerCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<RequestCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<RequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(AddCustomerCommand)));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_request_code_generate_as_post).Name));

                                      mockQuery = MockQuery<RequestCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(AddCustomerCommand))),
                                                         Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "AddCustomerCommand")
                                                                                                                            .Tuning(r => r.Package, "com.qabenchmarking.android.models.AddCustomerCommand")
                                                                                                                            .Tuning(r => r.Namespace, "com.qabenchmarking.android.models")))
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)
                                                                                                                   .Tuning(r => r.Type, query.Type)), "AddCustomerCommand")
                                              .StubQuery(Pleasure.Generator.Invent<GetUrlByTypeQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                                .Tuning(r => r.Type, query.Type)), "http://localhost/Dispatcher")
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(AddCustomerCommand))), new List<GetPropertiesByTypeQuery.Response>());
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}