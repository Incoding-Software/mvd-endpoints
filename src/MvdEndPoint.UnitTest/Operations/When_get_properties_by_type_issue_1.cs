namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetPropertiesQuery))]
    public class When_get_properties_by_type_issue_1
    {
        #region Establish value

        static MockMessage<GetPropertiesQuery, List<GetPropertiesQuery.Response>> mockQuery;

        #endregion

        private static string testResponseType;

        private static string pagingContainerType;

        Establish establish = () =>
                              {
                                  GetPropertiesQuery query = Pleasure.Generator.Invent<GetPropertiesQuery>(dsl => dsl.Tuning(r => r.Type, typeof(PagingResult<TestResponse>)));

                                  testResponseType = Pleasure.Generator.String();
                                  pagingContainerType = Pleasure.Generator.String();
                                  mockQuery = MockQuery<GetPropertiesQuery, List<GetPropertiesQuery.Response>>
                                          .When(query)
                                          .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(s => s.Type, typeof(TestResponse))
                                                                                                       .Tuning(r => r.Language, query.Language), testResponseType)
                                          .StubQuery<ConvertCSharpTypeToTargetQuery, string>(dsl => dsl.Tuning(s => s.Type, typeof(PagingContainer))
                                                                                                       .Tuning(r => r.Language, query.Language), pagingContainerType);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(result => result.ShouldEqualWeakEach(new[]
                                                                                                    {
                                                                                                            new GetPropertiesQuery.Response()
                                                                                                            {
                                                                                                                    Type = testResponseType,
                                                                                                                    Name = "Items",
                                                                                                                    Target = typeof(TestResponse),
                                                                                                                    Attributes = GetPropertiesQuery.Response.OfAttributes.IsCanNull | GetPropertiesQuery.Response.OfAttributes.IsArray | GetPropertiesQuery.Response.OfAttributes.IsClass
                                                                                                            },
                                                                                                            new GetPropertiesQuery.Response()
                                                                                                            {
                                                                                                                    Type = pagingContainerType,
                                                                                                                    Name = "Paging",
                                                                                                                    Attributes = GetPropertiesQuery.Response.OfAttributes.IsCanNull | GetPropertiesQuery.Response.OfAttributes.IsClass,
                                                                                                                    Target = typeof(PagingContainer)
                                                                                                            }
                                                                                                    }));

        public class TestResponse { }

        public class PagingContainer
        {
            public string Start { get; set; }

            public string End { get; set; }

            public string Total { get; set; }
        }

        public class PagingResult<TModel>
        {
            #region Properties

            public List<TModel> Items { get; set; }

            public PagingContainer Paging { get; set; }

            #endregion
        }
    }
}