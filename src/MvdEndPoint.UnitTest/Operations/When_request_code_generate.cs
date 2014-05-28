namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(DtoCodeGenerateQuery))]
    public class When_request_code_generate
    {
        #region Fake classes

        public class GetCustomerQuery : QueryBase<string>
        {
            #region Properties

            [UsedImplicitly]
            public string Message { get; set; }

            #endregion

            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<DtoCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<DtoCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Prefix, "Request")
                                                                                                            .Tuning(r => r.Type, typeof(GetCustomerQuery).AssemblyQualifiedName));
                                      expected = @" public class GetCustomerRequest {

    public TheSameString Message;
                                                     

 }";

                                      mockQuery = MockQuery<DtoCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))), new Dictionary<string, string>
                                                                                                                                                                       {
                                                                                                                                                                               { "Message", "TheSameString" }
                                                                                                                                                                       });
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}