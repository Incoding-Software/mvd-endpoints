namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(AndroidTaskCodeGenerateQuery))]
    public class When_android_task_code_generate_command
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidTaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand)));

                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_task_code_generate_command).Name));

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
                                                                                                                   .Tuning(r => r.IsCommand, true)
                                                                                                                   .Tuning(r => r.Type, typeof(FakeCommand))), new List<GetPropertiesByTypeQuery.Response>());
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));

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

        static MockMessage<AndroidTaskCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}