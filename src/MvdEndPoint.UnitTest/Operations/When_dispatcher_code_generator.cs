namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(DispatcherCodeGeneratorQuery))]
    public class When_dispatcher_code_generator
    {
        #region Fake classes

        public class FakeQuery { }

        public class Fake2Query { }

        #endregion

        #region Establish value

        static MockMessage<DispatcherCodeGeneratorQuery, string> mockQuery;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<DispatcherCodeGeneratorQuery>(dsl => dsl.Tuning(r => r.Types, new List<Type>
                                                                                                                                              {
                                                                                                                                                      typeof(FakeQuery), 
                                                                                                                                                      typeof(Fake2Query), 
                                                                                                                                              }));
                                      expected = @" public class Dispatcher {

     public CustomerTask  GetCustomer(CustomerRequest request) {
        return new  CustomerTask(request);
    }

    public ProductTask  GetProduct(ProductRequest request) {
        return new  ProductTask(request);
    }

        
}";
                                      Func<GetNameFromTypeQuery.ModeOf, Type, GetNameFromTypeQuery> getName = (modeOf, type) => Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, modeOf)
                                                                                                                                                                                          .Tuning(r => r.Type, type));

                                      mockQuery = MockQuery<DispatcherCodeGeneratorQuery, string>
                                              .When(query)
                                              .StubQuery(getName(GetNameFromTypeQuery.ModeOf.Method, typeof(FakeQuery)), "GetCustomer")
                                              .StubQuery(getName(GetNameFromTypeQuery.ModeOf.Task, typeof(FakeQuery)), "CustomerTask")
                                              .StubQuery(getName(GetNameFromTypeQuery.ModeOf.Request, typeof(FakeQuery)), "CustomerRequest")
                                              .StubQuery(getName(GetNameFromTypeQuery.ModeOf.Method, typeof(Fake2Query)), "GetProduct")
                                              .StubQuery(getName(GetNameFromTypeQuery.ModeOf.Task, typeof(Fake2Query)), "ProductTask")
                                              .StubQuery(getName(GetNameFromTypeQuery.ModeOf.Request, typeof(Fake2Query)), "ProductRequest");
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
    }
}