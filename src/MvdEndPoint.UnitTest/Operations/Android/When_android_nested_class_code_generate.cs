namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;
    using MvdEndPoint.Domain.Operations;

    #endregion

    [Subject(typeof(AndroidNestedClassCodeGenerateQuery))]
    public class When_android_nested_class_code_generate
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<AndroidNestedClassCodeGenerateQuery>();
                                  expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_android_nested_class_code_generate).Name));

                                  mockQuery = MockQuery<AndroidNestedClassCodeGenerateQuery, string>
                                          .When(query)
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Tuning(r => r.IsCommand, false)
                                                                                                                      .Tuning(r => r.Device, DeviceOfType.Android)
                                                                                                                      .Tuning(r => r.Type, query.Type)
                                                                                                            , Pleasure.ToList(Pleasure.Generator.Invent<GetPropertiesQuery.Response>(dsl => dsl.Tuning(r => r.Name, "ContactId"))))
                                          .StubQuery<GetNameFromTypeQuery, Dictionary<GetNameFromTypeQuery.ModeOf, string>>(dsl => dsl.Tuning(r => r.Type, query.Type), new Dictionary<GetNameFromTypeQuery.ModeOf, string>()
                                                                                                                                                                        {
                                                                                                                                                                                { GetNameFromTypeQuery.ModeOf.Nested, "ClassName" }
                                                                                                                                                                        });
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(r => r.ShouldEqual(expected));

        #region Establish value

        static MockMessage<AndroidNestedClassCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion
    }
}