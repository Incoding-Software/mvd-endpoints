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

        public class FakeQuery : QueryBase<FakeQuery.Response>
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
public class FakeTask extends AsyncTask<String, Integer, String> {

    public IFakeOn listener;

    private FakeRequest request;

    public FakeTask(FakeRequest request) {
        this.request = request;
    }

    @Override
    protected void onPostExecute(String s) {
        super.onPostExecute(s);


        try {
            Response result = new Response();

            JSONObject jsonObject = new JSONObject(s);
            JSONObject data = new JSONObject(jsonObject.getString(""data""));
                             result.Name = data.getString(""Name"");
                   

            listener.Success(result);

        } catch (JSONException e) {
            e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
        }

    }


    @Override
    protected String doInBackground(String... strings) {
        HttpClient httpClient = new DefaultHttpClient();
        String uri = String.format(""http://mvd-endpoint.incframework.com//Dispatcher/Query?incType=GetCustomerByCriteriaQuery&Message=%"",request.Message);
        HttpGet httpGet = new HttpGet(uri);
        try {
            HttpResponse response = httpClient.execute(httpGet);
            String json = EntityUtils.toString(response.getEntity());
            return json;
        } catch (IOException e) {
            e.printStackTrace();
        }
        return """";
    }


    public void On(IGetCustomerByCriteriaOn on)
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
                                                                                                                                                                             { "Name", "Message" }
                                                                                                                                                                     })
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery.Response))), new Dictionary<string, string>
                                                                                                                                                                              {
                                                                                                                                                                                      { "Name", "Message" }
                                                                                                                                                                              });
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}