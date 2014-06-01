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

    [Subject(typeof(TaskCodeGenerateQuery))]
    public class When_task_code_generate
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Properties

            [UsedImplicitly]
            public string Id { get; set; }

            #endregion

            #region Nested classes

            public class Response
            {
                #region Properties

                [UsedImplicitly]
                public string Name { get; set; }

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

        static MockMessage<TaskCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<TaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));

                                      expected = @"
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.preference.PreferenceManager;
import org.apache.http.Header;
import org.apache.http.HttpResponse;
import org.apache.http.util.EntityUtils;
import org.json.JSONObject;

public class FakeTask extends AsyncTask<String, Integer, String> {

    private IFakeOn listener;

    private FakeRequest request ;

	    public FakeTask(FakeRequest request) {    
	this.request = request;    
    }
	 
	
	@Override
    protected void onPostExecute(String s) {
        super.onPostExecute(s);
        try {
            JSONObject jsonObject = new JSONObject(s);
            JSONObject data = new JSONObject(jsonObject.getString(""data""));
            listener.Success(Response.Create(data));
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

	@Override
    protected String doInBackground(String... strings) {
        try {
            HttpResponse response = request.execute();
            Header[] cookieHeader = response.getHeaders(""Set-Cookie"");
            SharedPreferences preferences = PreferenceManager.getDefaultSharedPreferences(new MainActivity());
            for (Header header : cookieHeader) {
                preferences.edit().putString(header.getName(), header.getValue());
            }
            String json = EntityUtils.toString(response.getEntity());
            return json;
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
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))), new Dictionary<string, string>
                                                                                                                                                                     {
                                                                                                                                                                             { "Id", ConvertCSharpTypeToJavaQuery.String },
                                                                                                                                                                             { "Title", ConvertCSharpTypeToJavaQuery.String },
                                                                                                                                                                     })
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery.Response))), new Dictionary<string, string>());
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}