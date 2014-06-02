namespace MvdEndPoint.UnitTest
{
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;
    using It = Machine.Specifications.It;

    [Subject(typeof(ModelStateExceptionCodeGenerateQuery))]
    public class When_model_state_exception_code_generate
    {
        #region Establish value

        static MockMessage<ModelStateExceptionCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      ModelStateExceptionCodeGenerateQuery query = Pleasure.Generator.Invent<ModelStateExceptionCodeGenerateQuery>();
                                      expected = @"
public class ModelStateException extends Throwable {
    private JsonModelStateData[] state;

    public ModelStateException(JsonModelStateData[] state) {
        this.state = state;
    }

    public JsonModelStateData[] getState() {
        return state;
    }
}
";

                                      mockQuery = MockQuery<ModelStateExceptionCodeGenerateQuery, string>
                                              .When(query);

                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}