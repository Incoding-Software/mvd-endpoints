﻿namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AndroidTaskCodeGenerateQuery))]
    public class When_android_task_code_generate_without_request
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Nested classes

            public class Response { }

            #endregion

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<AndroidTaskCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidTaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeQuery)));

                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_task_code_generate_without_request).Name));

                                  var meta = Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models")
                                                                                                                .Tuning(r => r.IsCommand, false)
                                                                                                                .Tuning(r => r.Package, "com.qabenchmarking.android.models.Fake"));
                                  mockQuery = MockQuery<AndroidTaskCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), 
                                                     meta)
                                          .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                  {
                                                                                                                                                          { GetNameFromTypeQuery.ModeOf.Listener, "IFakeOn" }, 
                                                                                                                                                          { GetNameFromTypeQuery.ModeOf.Request, "FakeRequest" }, 
                                                                                                                                                          { GetNameFromTypeQuery.ModeOf.Response, "Response" }, 
                                                                                                                                                          { GetNameFromTypeQuery.ModeOf.Task, "FakeTask" }, 
                                                                                                                                                  })
                                          .StubQuery(Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Android)
                                                                                                                     .Tuning(r => r.IsCommand, meta.IsCommand)
                                                                                                                     .Tuning(r => r.Type, typeof(FakeQuery))), new List<GetPropertiesQuery.Response>());
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}