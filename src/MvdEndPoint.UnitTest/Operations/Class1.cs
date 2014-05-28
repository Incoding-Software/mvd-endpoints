namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(OnCodeGeneratorQuery))]
    public class When_on_code_generator
    {
        #region Fake classes

        class FakeQuery
        {
            #region Properties

            public string Type { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<OnCodeGeneratorQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<OnCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));
                                      expected = @"
public interface FakeQueryOn {
    void Success(FakeQueryResponse response);
}";

                                      mockQuery = MockQuery<OnCodeGeneratorQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Listener)), "FakeQueryOn")
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                                                   .Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Response)), "FakeQueryResponse");
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}