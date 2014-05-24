namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(RequestCodeGenerateQuery))]
    public class When_request_code_generate
    {
        #region Fake classes

        public class GetCustomerQuery : QueryBase<string>
        {
            #region Properties

            public string Message { get; set; }

            #endregion

            protected override string ExecuteResult()
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
                                      var query = Pleasure.Generator.Invent<RequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.AssemblyQualifiedType, typeof(GetCustomerQuery).AssemblyQualifiedName));
                                      expected = @" public class GetCustomerRequest {

    public String Message
                                                     

 }";

                                      mockQuery = MockQuery<RequestCodeGenerateQuery, string>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}