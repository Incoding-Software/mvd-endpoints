namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(ResponseCodeGenerateQuery))]
    public class When_response_code_generate_as_command
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<ResponseCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<ResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand)));
                                      expected = @"
import org.json.JSONException;
import org.json.JSONObject;	

public class AddCustomerResponse {

  public static GetCustomerResponse Create(JSONObject data) throws JSONException { 
    Object result = new Object();
    result = data;       
    return result;  
   }              
                                                     
}";

                                      mockQuery = MockQuery<ResponseCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)
                                                                                                                   .Tuning(r => r.Type, query.Type)), "AddCustomerResponse")
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand))), new Dictionary<string, string>());
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}