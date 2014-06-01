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

    [Subject(typeof(RequestCodeGenerateQuery))]
    public class When_request_code_generate_as_get
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

        static MockMessage<RequestCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<RequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery)));
                                      expected = @"
import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;

public class GetCustomerRequest {

    public TheSameString Message;
    public Number Title;
     
     public HttpResponse execute() throws IOException {        	
		String uri = String.format(""http://localhost/Dispatcher"" ,this.Message  ,this.Title ); 
        		        HttpGet http = new HttpGet(uri);
        return new DefaultHttpClient().execute(http);
    }
                                                        
}";

                                      mockQuery = MockQuery<RequestCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)
                                                                                                                   .Tuning(r => r.Type, query.Type)), "GetCustomerRequest")
                                              .StubQuery(Pleasure.Generator.Invent<GetUrlByTypeQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                                .Tuning(r => r.Type, query.Type)), "http://localhost/Dispatcher")
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))), new Dictionary<string, string>
                                                                                                                                                                            {
                                                                                                                                                                                    { "Message", "TheSameString" },
                                                                                                                                                                                    { "Title", "Number" }
                                                                                                                                                                            });
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}