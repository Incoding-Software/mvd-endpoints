namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(GetNameFromTypeQuery))]
    public class When_get_name_from_type
    {
        #region Fake classes

        class FakeQuery : QueryBase<When_task_code_generate.FakeQuery.Response>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override When_task_code_generate.FakeQuery.Response ExecuteResult()
            {
                return null;
            }
        }

        #endregion

        #region Establish value

        static void Compare(GetNameFromTypeQuery.ModeOf modeOf, string title)
        {
            var query = Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery))
                                                                                  .Tuning(r => r.Mode, modeOf));
            query.Execute();
            query.Result.ShouldEqual(title);
        }

        #endregion

        It should_be_method = () => Compare(GetNameFromTypeQuery.ModeOf.Method, "FakeQuery");

        It should_be_request = () => Compare(GetNameFromTypeQuery.ModeOf.Request, "FakeQueryRequest");

        It should_be_listener = () => Compare(GetNameFromTypeQuery.ModeOf.Listener, "IFakeQueryOn");

        It should_be_task = () => Compare(GetNameFromTypeQuery.ModeOf.Task, "FakeQueryTask");

        It should_be_response = () => Compare(GetNameFromTypeQuery.ModeOf.Response, "FakeQuery_Response");
    }
}