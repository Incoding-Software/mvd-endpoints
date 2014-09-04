﻿namespace MvdEndPoint.UnitTest
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

    [Subject(typeof(AndroidRequestCodeGenerateQuery))]
    public class When_android_request_code_generate_as_get
    {
        #region Fake classes

        class GetCustomerQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<AndroidRequestCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<AndroidRequestCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery)));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_request_code_generate_as_get).Name));

                                      mockQuery = MockQuery<AndroidRequestCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetMetaFromTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(GetCustomerQuery))),
                                                         Pleasure.Generator.Invent<GetMetaFromTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "GetCustomerQuery")
                                                                                                                            .Tuning(r => r.Package, "com.qabenchmarking.android.models.GetCustomerQuery")
                                                                                                                            .Tuning(r => r.Namespace, "com.qabenchmarking.android.models")))
                                              .StubQuery(Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, GetNameFromTypeQuery.ModeOf.Request)
                                                                                                                   .Tuning(r => r.Type, query.Type)), "GetCustomerRequest")
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Android)
                                                                                                                       .Tuning(r => r.Type, typeof(GetCustomerQuery))), new List<GetPropertiesByTypeQuery.Response>
                                                                                                                                                                            {
                                                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Message")
                                                                                                                                                                                                                                                           .Tuning(r => r.IsCanNull, true)
                                                                                                                                                                                                                                                           .Tuning(r => r.IsArray, false)
                                                                                                                                                                                                                                                           .Tuning(r => r.Type, "TheSameString")),
                                                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Title")
                                                                                                                                                                                                                                                           .Tuning(r => r.IsCanNull, false)
                                                                                                                                                                                                                                                           .Tuning(r => r.IsArray, false)
                                                                                                                                                                                                                                                           .Tuning(r => r.Type, "Number")),
                                                                                                                                                                                    Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>(dsl => dsl.Tuning(r => r.Name, "Ids")
                                                                                                                                                                                                                                                           .Tuning(r => r.IsCanNull, false)
                                                                                                                                                                                                                                                           .Tuning(r => r.IsArray, true)
                                                                                                                                                                                                                                                           .Tuning(r => r.Type, "String"))
                                                                                                                                                                            });
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}