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

    [Subject(typeof(GetInnerResponseTypesQuery))]
    public class When_get_inner_response_types
    {
        #region Fake classes

        

        public class Res1 { }

        public class Res2 { }

        public class ComplexityResponse:QueryBase<String>
        {
            #region Properties

            public Res1 Reference { get; set; }

            public Res1[] Many { get; set; }

            public Res2 SecondReference { get; set; }

            public string AsString { get; set; }

            public int AsInt { get; set; }

            public int[] AsArrayInt { get; set; }

            #endregion

            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<GetInnerResponseTypesQuery, Dictionary<string, List<GetPropertiesQuery.Response>>> mockQuery;

        static List<GetPropertiesQuery.Response> propertiesOfRes1;

        static List<GetPropertiesQuery.Response> propertiesOfRes2;

        #endregion

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<GetInnerResponseTypesQuery>(dsl => dsl.Tuning(r => r.Type, typeof(ComplexityResponse)));

                                  propertiesOfRes1 = Pleasure.Generator.Invent<List<GetPropertiesQuery.Response>>();
                                  propertiesOfRes2 = Pleasure.Generator.Invent<List<GetPropertiesQuery.Response>>();
                                  mockQuery = MockQuery<GetInnerResponseTypesQuery, Dictionary<string, List<GetPropertiesQuery.Response>>>
                                          .When(query)
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Tuning(r => r.Type, typeof(Res1))
                                                                                                                      .Tuning(r => r.Device, DeviceOfType.WP)
                                                                                                                      .Tuning(r => r.IsCommand, false), propertiesOfRes1)
                                          .StubQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>(dsl => dsl.Tuning(r => r.Type, typeof(Res2))
                                                                                                                      .Tuning(r => r.Device, DeviceOfType.WP)
                                                                                                                      .Tuning(r => r.IsCommand, false), propertiesOfRes2);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(new Dictionary<string, List<GetPropertiesQuery.Response>>()
                                                               {
                                                                       { "Res1", propertiesOfRes1 }, 
                                                                       { "Res2", propertiesOfRes2 }
                                                               });
    }
}