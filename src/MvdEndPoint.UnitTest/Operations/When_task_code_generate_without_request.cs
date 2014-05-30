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

    [Subject(typeof(TaskCodeGenerateQuery))]
    public class When_task_code_generate_without_request
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<TaskCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<TaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, "http://test.com")
                                                                                                             .Tuning(r => r.Type, typeof(FakeQuery)));

                                      expected = @"
import android.os.AsyncTask;
import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONException;
import org.json.JSONObject;
import java.io.IOException;

public class FakeTask extends AsyncTask<String, Integer, String> {

    public IFakeOn listener;

     

    public FakeTask(  ) {
     
    }

    @Override
    protected void onPostExecute(String s) {
        super.onPostExecute(s);


        try {
            Response result = new Response();

            JSONObject jsonObject = new JSONObject(s);
            JSONObject data = new JSONObject(jsonObject.getString(""data""));
                   

            listener.Success(result);

        } catch (JSONException e) {
            e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
        }

    }


    @Override
    protected String doInBackground(String... strings) {        		
		
						String uri = ""http://localhost/Dispatcher"";
        		
        HttpGet http = new HttpGet(uri);         
        
            
        try {
            HttpResponse response = new DefaultHttpClient().execute(http);
            String json = EntityUtils.toString(response.getEntity());
            return json;
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return """";
    }


    public void On(IFakeOn on)
    {
        listener = on;
        execute();
    }
}";

                                      Func<GetNameFromTypeQuery.ModeOf, GetNameFromTypeQuery> createByName = modeOf => Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, modeOf)
                                                                                                                                                                                 .Tuning(r => r.Type, query.Type));

                                      mockQuery = MockQuery<TaskCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Listener), "IFakeOn")
                                              .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Request), "FakeRequest")
                                              .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Response), "Response")
                                              .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Task), "FakeTask")
                                              .StubQuery(Pleasure.Generator.Invent<GetUrlByTypeQuery>(dsl => dsl.Tuning(r => r.BaseUrl, query.BaseUrl)
                                                                                                                .Tuning(r => r.Type, query.Type)), "http://localhost/Dispatcher")
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))), new Dictionary<string, string>())
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery.Response))), new Dictionary<string, string>());
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}