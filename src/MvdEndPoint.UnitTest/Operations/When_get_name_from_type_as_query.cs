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
    public class When_get_name_from_type_as_query
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Nested classes

            public class Response { }

            #endregion

            #region Enums

            public enum NestedEnum
            { }

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
            var query = Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, type ?? typeof(FakeQuery)));
            var mock = MockQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>.When(query);
            mock.Execute();
            mock.ShouldBeIsResult(dictionary => dictionary[modeOf].ShouldEqual(title));
        }

        #endregion

        It should_be_request = () => Compare(GetNameFromTypeQuery.ModeOf.Request, "FakeQueryRequest");

        It should_be_listener = () => Compare(GetNameFromTypeQuery.ModeOf.Listener, "IFakeQueryListener");

        It should_be_task = () => Compare(GetNameFromTypeQuery.ModeOf.Task, "FakeQueryTask");

        It should_be_response = () => Compare(GetNameFromTypeQuery.ModeOf.Response, "FakeQueryResponse");

        It should_be_enum = () => Compare(GetNameFromTypeQuery.ModeOf.Enum, "OuterEnum", typeof(OuterEnum));

        It should_be_enum_as_nested = () => Compare(GetNameFromTypeQuery.ModeOf.Enum, "When_get_name_from_type_as_query_FakeQuery_NestedEnum", typeof(FakeQuery.NestedEnum));

        It should_be_response_as_list = () => Compare(GetNameFromTypeQuery.ModeOf.Response, "FakeWithListQueryResponse", type: typeof(FakeWithListQuery));
    }
}