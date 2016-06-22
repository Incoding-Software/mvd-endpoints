namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetNameFromTypeQuery))]
    public class When_get_name_from_type_as_command
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Compare(GetNameFromTypeQuery.ModeOf modeOf, string title, Type type = null)
        {
            var query = Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, type ?? typeof(FakeCommand)));
            var mock = MockQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>.When(query);
            mock.Execute();
            mock.ShouldBeIsResult(dictionary => dictionary[modeOf].ShouldEqual(title));
        }

        #endregion

        It should_be_request = () => Compare(GetNameFromTypeQuery.ModeOf.Request, "FakeCommandRequest");

        It should_be_listener = () => Compare(GetNameFromTypeQuery.ModeOf.Listener, "IFakeCommandListener");
        
        It should_be_response = () => Compare(GetNameFromTypeQuery.ModeOf.Response, "FakeCommandResponse");
    }
}