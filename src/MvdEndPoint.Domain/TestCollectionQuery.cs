namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;

    #endregion

    public class TestCollectionQuery : QueryBase<List<TestCollectionQuery.Response>>
    {
        #region Nested classes

        public class Response
        {
            #region Properties

            public string Title { get; set; }

            #endregion
        }

        #endregion

        protected override List<Response> ExecuteResult()
        {
            return new List<Response>
                       {
                               new Response { Title = "Audi" }, 
                               new Response { Title = "BMW" }
                       };
        }
    }
}