namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetNameFromTypeQuery))]
    public class When_get_name_from_type_as_command
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Compare(GetNameFromTypeQuery.ModeOf modeOf, string title, Type type = null)
        {
            var query = Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, type ?? typeof(FakeCommand))
                                                                                  .Tuning(r => r.Mode, modeOf));
            query.Execute();
            query.Result.ShouldEqual(title);
        }

        #endregion

        It should_be_request = () => Compare(GetNameFromTypeQuery.ModeOf.Request, "FakeCommandRequest");

        It should_be_listener = () => Compare(GetNameFromTypeQuery.ModeOf.Listener, "IFakeCommandListener");

        It should_be_task = () => Compare(GetNameFromTypeQuery.ModeOf.Task, "FakeCommandTask");

        It should_be_response = () => Compare(GetNameFromTypeQuery.ModeOf.Response, "Object");
    }
}