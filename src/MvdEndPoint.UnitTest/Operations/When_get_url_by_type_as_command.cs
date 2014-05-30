namespace MvdEndPoint.UnitTest
{
    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    [Subject(typeof(GetUrlByTypeQuery))]
    public class When_get_url_by_type_as_command
    {
        #region Fake 

        class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<GetUrlByTypeQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<GetUrlByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand))
                                                                                                         .Tuning(r => r.BaseUrl, "http://localhost:48801"));
                                      expected = "http://localhost:48801/Dispatcher/Push?incType=FakeCommand";

                                      mockQuery = MockQuery<GetUrlByTypeQuery, string>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}