﻿namespace MvdEndPoint.UnitTest
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

    [Subject(typeof(ResponseCodeGenerateQuery))]
    public class When_response_code_generate
    {
        #region Fake classes

        public class GetCustomerQuery : QueryBase<GetCustomerQuery.Response>
        {
            #region Nested classes

            public class Response
            {
                #region Properties

                [UsedImplicitly]
                public string Message { get; set; }

                #endregion
            }

            #endregion

            protected override Response ExecuteResult()
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
                                      var query = Pleasure.Generator.Invent<ResponseCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery)));
                                      expected = @"
import org.json.JSONException;
import org.json.JSONObject;	

public class GetCustomerResponse {

      public String Title;
    public int Number;
      
    public static GetCustomerResponse Create(JSONObject data) throws JSONException { 
    	GetCustomerResponse result = new GetCustomerResponse();
    result.Title = data.getString(""Title"");
    result.Number = data.getInt(""Number"");
   
    return result;  
	  }              
                                                     
}";

                                      mockQuery = MockQuery<ResponseCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)
                                                                                                                   .Tuning(r => r.Type, query.Type)), "GetCustomerResponse")
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery.Response))), new Dictionary<string, string>
                                                                                                                                                                                     {
                                                                                                                                                                                             { "Title", ConvertCSharpTypeToJavaQuery.String },
                                                                                                                                                                                             { "Number", ConvertCSharpTypeToJavaQuery.Int },
                                                                                                                                                                                     });
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}