namespace MvdEndPoint.Domain
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class TestQuery : QueryBase<TestQuery.Response>
    {
        #region Nested classes

        public class Response
        {
            #region Properties

            public string Title { get; set; }

            #endregion
        }

        #endregion

        protected override Response ExecuteResult()
        {
            return new Response
                       {
                               Title = "Audi"
                       };
        }
    }
}