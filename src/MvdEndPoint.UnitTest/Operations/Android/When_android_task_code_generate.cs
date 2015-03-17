namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(AndroidTaskCodeGenerateQuery))]
    public class When_android_task_code_generate
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidTaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_task_code_generate).Name));

                                  Func<GetNameFromTypeQuery.ModeOf, GetNameFromTypeQuery> createByName = modeOf => Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, modeOf)
                                                                                                                                                                             .Tuning(r => r.Type, query.Type));

                                  mockQuery = MockQuery<AndroidTaskCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)),
                                                     Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models")
                                                                                                                        .Tuning(r => r.Package, "com.qabenchmarking.android.models.Fake")))
                                          .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Listener), "IFakeOn")
                                          .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Request), "FakeRequest")
                                          .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Response), "Response")
                                          .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Task), "FakeTask")
                                          .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Android)
                                                                                                                   .Tuning(r => r.IsCommand, false)
                                                                                                                   .Tuning(r => r.Type, typeof(FakeQuery))), new List<GetPropertiesByTypeQuery.Response> { Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>() });
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Properties

            [UsedImplicitly]
            public string Id { get; set; }

            #endregion

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }

            #region Nested classes

            public class Response
            {
                #region Properties

                [UsedImplicitly]
                public string Name { get; set; }

                #endregion
            }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<AndroidTaskCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}