namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(IncodingHelperCodeGenerateQuery))]
    public class When_incoding_helper_code_generate
    {
        #region Establish value

        static MockMessage<IncodingHelperCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<IncodingHelperCodeGenerateQuery>();
                                      expected = @"
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class IncodingHelper
{
    public static void Verify(JSONObject result) throws JSONException, ModelStateException {
        if (!result.getBoolean(""success"")) {
            JSONArray errors = result.isNull(""data"") ? new JSONArray() : result.getJSONArray(""data"");
            JsonModelStateData[] state = new JsonModelStateData[errors.length()];
            for (int i = 0; i < errors.length(); i++) {
                JSONObject itemError = errors.getJSONObject(i);
                JsonModelStateData jsonModelStateData = new JsonModelStateData();
                jsonModelStateData.errorMessage = itemError.getString(""errorMessage"");
                jsonModelStateData.isValid = itemError.getBoolean(""isValid"");
                jsonModelStateData.name = itemError.getString(""name"");
                state[i] = jsonModelStateData;
            }
            throw new ModelStateException(state);
        }
    }
}
";

                                      mockQuery = MockQuery<IncodingHelperCodeGenerateQuery, string>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}