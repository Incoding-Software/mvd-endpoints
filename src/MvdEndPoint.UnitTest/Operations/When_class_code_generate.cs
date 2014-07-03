namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(ClassCodeGenerateQuery))]
    public class When_class_code_generate
    {
        #region Establish value

        static MockMessage<ClassCodeGenerateQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<ClassCodeGenerateQuery>(dsl => dsl.Tuning(r => r.Namespace, "com.qabenchmarking.android.models")
                                          .Tuning(r => r.Type, typeof(ClassCodeGenerateQuery)));
                                      expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_class_code_generate).Name));

                                      mockQuery = MockQuery<ClassCodeGenerateQuery, string>
                                              .When(query)
                                              .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Type, query.Type)), new List<GetPropertiesByTypeQuery.Response>
                                                                                                                                                              {
                                                                                                                                                                      new GetPropertiesByTypeQuery.Response { Name = "Name", Type = "String" },
                                                                                                                                                                      new GetPropertiesByTypeQuery.Response { Name = "Number", Type = "Int" }
                                                                                                                                                              });
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}