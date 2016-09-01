namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Web;
    using CloudIn.Domain.Endpoint;
    using Incoding.Endpoint;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetMessageDetailQuery))]
    public class When_get_message_detail
    {
        public enum TestEnum
        { }

        It should_be_date = () =>
                            {
                                Run(typeof(DateTime), response => response.ShouldEqualWeakEach(expected, (dsl, i) => dsl.ForwardToValue(r => r.IsDate, true)
                                                                                                                        .ForwardToValue(r => r.IsEnum, false)
                                                                                                                        .ForwardToValue(r => r.IsBool, false)
                                                                                                                        .ForwardToValue(r => r.GUID, typeof(DateTime).GUID)
                                                                                                                        .ForwardToValue(r => r.IsNumber, false)
                                                                                                                        .ForwardToValue(r => r.IsString, false)
                                                                                                                        .ForwardToValue(r => r.IsArray, false)
                                                                                                                        .ForwardToValue(r => r.IsFile, false)));
                            };

        It should_be_enum = () =>
                            {
                                Run(typeof(TestEnum), response => response.ShouldEqualWeakEach(expected, (dsl, i) => dsl.ForwardToValue(r => r.IsBool, false)
                                                                                                                        .ForwardToValue(r => r.IsEnum, true)
                                                                                                                        .ForwardToValue(r => r.GUID, typeof(TestEnum).GUID)
                                                                                                                        .ForwardToValue(r => r.IsDate, false)
                                                                                                                        .ForwardToValue(r => r.IsNumber, false)
                                                                                                                        .ForwardToValue(r => r.IsArray, false)
                                                                                                                        .ForwardToValue(r => r.IsString, false)
                                                                                                                        .ForwardToValue(r => r.IsFile, false)));
                            };

        It should_be_file = () =>
                            {
                                Action<ICompareFactoryDsl<GetMessageDetailQuery.Response, Message.Property>, int> action = (dsl, i) => dsl.ForwardToValue(r => r.IsBool, false)
                                                                                                                                          .ForwardToValue(r => r.IsEnum, false)
                                                                                                                                          .ForwardToValue(r => r.IsDate, false)
                                                                                                                                          .IgnoreBecauseCalculate(r => r.GUID)
                                                                                                                                          .ForwardToValue(r => r.IsArray, false)
                                                                                                                                          .ForwardToValue(r => r.IsNumber, false)
                                                                                                                                          .ForwardToValue(r => r.IsString, false)
                                                                                                                                          .ForwardToValue(r => r.IsFile, true);
                                Run(typeof(byte[]), response => response.ShouldEqualWeakEach(expected, action));
                                Run(typeof(HttpPostedFileBase), response => response.ShouldEqualWeakEach(expected, action));
                                Run(typeof(HttpPostedFile), response => response.ShouldEqualWeakEach(expected, action));
                            };

        It should_be_string = () =>
                              {
                                  Run(typeof(string), response => response.ShouldEqualWeakEach(expected, (dsl, i) => dsl.ForwardToValue(r => r.IsBool, false)
                                                                                                                        .ForwardToValue(r => r.IsEnum, false)
                                                                                                                        .ForwardToValue(r => r.GUID, typeof(string).GUID)
                                                                                                                        .ForwardToValue(r => r.IsDate, false)                                                                                                                        
                                                                                                                        .ForwardToValue(r => r.IsArray, false)
                                                                                                                        .ForwardToValue(r => r.IsNumber, false)
                                                                                                                        .ForwardToValue(r => r.IsString, true)
                                                                                                                        .ForwardToValue(r => r.IsFile, false)));
                              };

        static void Run(Type type, Action<List<GetMessageDetailQuery.Response>> verifyResult)
        {
            GetMessageDetailQuery query = Pleasure.Generator.Invent<GetMessageDetailQuery>();
            expected = Pleasure.ToArray(Pleasure.Generator.Invent<Message.Property>(factoryDsl => factoryDsl.Tuning(r => r.Type, Message.Property.TypeOf.Request)));

            mockQuery = MockQuery<GetMessageDetailQuery, List<GetMessageDetailQuery.Response>>
                    .When(query)
                    .StubQuery(whereSpecification: new Message.Property.Where.ByMessage(query.Id)
                                       .And(new Message.Property.Where.ByType(Message.Property.TypeOf.Request)),
                               entities: expected)
                    .StubQuery<GetTypeFromPropertyQuery, Type>(dsl => dsl.Tuning(s => s.Property, expected[0]), type);
            mockQuery.Original.Execute();

            mockQuery.ShouldBeIsResult(verifyResult);
        }

        #region Establish value

        static MockMessage<GetMessageDetailQuery, List<GetMessageDetailQuery.Response>> mockQuery;

        static Message.Property[] expected;

        #endregion
    }
}