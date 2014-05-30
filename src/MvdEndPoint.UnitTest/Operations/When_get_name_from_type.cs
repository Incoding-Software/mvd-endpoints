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

    [Subject(typeof(GetNameFromTypeQuery))]
    public class When_get_name_from_type
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override Response ExecuteResult()
            {
                return null;
            }
        }

        class FakeWithListQuery : QueryBase<List<FakeWithListQuery.Response>>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override List<Response> ExecuteResult()
            {
                return null;
            }
        }

        #endregion

        #region Establish value

        static void Compare(GetNameFromTypeQuery.ModeOf modeOf, string title, Type type = null)
        {
            var query = Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, type ?? typeof(FakeQuery))
                                                                                  .Tuning(r => r.Mode, modeOf));
            query.Execute();
            query.Result.ShouldEqual(title);
        }

        #endregion

        It should_be_request = () => Compare(GetNameFromTypeQuery.ModeOf.Request, "FakeQueryRequest");

        It should_be_listener = () => Compare(GetNameFromTypeQuery.ModeOf.Listener, "IFakeQueryListener");

        It should_be_task = () => Compare(GetNameFromTypeQuery.ModeOf.Task, "FakeQueryTask");

        It should_be_response = () => Compare(GetNameFromTypeQuery.ModeOf.Response, "FakeQueryResponse");

        It should_be_response_as_list = () => Compare(GetNameFromTypeQuery.ModeOf.Response, "FakeWithListQueryResponse", type: typeof(FakeWithListQuery));
    }
}