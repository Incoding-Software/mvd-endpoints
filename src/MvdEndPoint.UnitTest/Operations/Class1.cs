namespace MvdEndPoint.UnitTest
{
    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;
    using It = Machine.Specifications.It;

    [Subject(typeof(TaskCodeGenerateQuery))]
    public class When_task_code_generate
    {
        public class FakeQuery : QueryBase<FakeQuery.Response>
        {
            public string Id { get; set; }

            public class Response
            {
                public string Name { get; set; }
            }

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #region Establish value

        static MockMessage<TaskCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<TaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery).AssemblyQualifiedName));
                                      expected = "";

                                      mockQuery = MockQuery<TaskCodeGenerateQuery, string>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}