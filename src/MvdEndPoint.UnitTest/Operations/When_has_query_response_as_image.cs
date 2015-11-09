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

    [Subject(typeof(HasQueryResponseAsImageQuery))]
    public class When_has_query_response_as_image
    {
        #region Fake classes

        public class FakeCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class FakeQuery : QueryBase<List<string>>
        {
            protected override List<string> ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        public class ImageQuery : QueryBase<byte[]>
        {
            protected override byte[] ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        It should_be_command = () =>
                               {
                                   var query = Pleasure.Generator.Invent<HasQueryResponseAsImageQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand)));

                                   var mockQuery = MockQuery<HasQueryResponseAsImageQuery, bool>
                                           .When(query)
                                           .StubQuery<IsCommandTypeQuery, bool>(dsl => dsl.Tuning(r => r.Type, query.Type), true);
                                   mockQuery.Execute();
                                   mockQuery.ShouldBeIsResult(false);
                               };

        It should_be_query = () =>
                             {
                                 var query = Pleasure.Generator.Invent<HasQueryResponseAsImageQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));

                                 var mockQuery = MockQuery<HasQueryResponseAsImageQuery, bool>
                                         .When(query)
                                         .StubQuery<IsCommandTypeQuery, bool>(dsl => dsl.Tuning(r => r.Type, query.Type), false);
                                 mockQuery.Execute();
                                 mockQuery.ShouldBeIsResult(false);
                             };

        It should_be_image = () =>
                             {
                                 var query = Pleasure.Generator.Invent<HasQueryResponseAsImageQuery>(dsl => dsl.Tuning(r => r.Type, typeof(ImageQuery)));

                                 var mockQuery = MockQuery<HasQueryResponseAsImageQuery, bool>
                                         .When(query)
                                         .StubQuery<IsCommandTypeQuery, bool>(dsl => dsl.Tuning(r => r.Type, query.Type), false);
                                 mockQuery.Execute();
                                 mockQuery.ShouldBeIsResult(true);
                             };
    }
}