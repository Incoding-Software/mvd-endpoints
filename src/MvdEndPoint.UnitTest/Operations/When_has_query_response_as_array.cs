namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(HasQueryResponseAsArrayQuery))]
    public class When_has_query_response_as_array
    {
        #region Fake classes

        public class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class QueryWithString : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        public class QueryWithBytes : QueryBase<byte[]>
        {
            protected override byte[] ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        public class QueryWithArray : QueryBase<QueryWithArray.Response[]>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override Response[] ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        It should_be_is_string = () =>
                                     {
                                         var query = Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, typeof(QueryWithString)));
                                         var mock = MockQuery<HasQueryResponseAsArrayQuery, IncBoolResponse>
                                                 .When(query);
                                         mock.Original.Execute();
                                         mock.ShouldBeIsResult(false);
                                     };

        It should_be_is_bytes = () =>
                                    {
                                        var query = Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, typeof(QueryWithBytes)));
                                        var mock = MockQuery<HasQueryResponseAsArrayQuery, IncBoolResponse>
                                                .When(query);
                                        mock.Original.Execute();
                                        mock.ShouldBeIsResult(false);
                                    };

        It should_be_is_command = () =>
                                      {
                                          var query = Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand)));
                                          var mock = MockQuery<HasQueryResponseAsArrayQuery, IncBoolResponse>
                                                  .When(query);
                                          mock.Original.Execute();
                                          mock.ShouldBeIsResult(false);
                                      };

        It should_be_is_array = () =>
                                    {
                                        var query = Pleasure.Generator.Invent<HasQueryResponseAsArrayQuery>(dsl => dsl.Tuning(r => r.Type, typeof(QueryWithArray)));
                                        var mock = MockQuery<HasQueryResponseAsArrayQuery, IncBoolResponse>
                                                .When(query);
                                        mock.Original.Execute();
                                        mock.ShouldBeIsResult(true);
                                    };
    }
}