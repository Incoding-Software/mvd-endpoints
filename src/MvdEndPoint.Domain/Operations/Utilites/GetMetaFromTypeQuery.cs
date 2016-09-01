namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetMetaFromTypeQuery : QueryBase<GetMetaFromTypeQuery.Response>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override Response ExecuteResult()
        {
            var serviceContract = Type.FirstOrDefaultAttribute<ServiceContractAttribute>();
            var isContract = serviceContract != null;
            string @namespace = "Incoding";

            return new Response
                   {
                           Name = Type.Name,
                           ResponseAsArray = isContract && Dispatcher.Query(new HasQueryResponseAsArrayQuery(Type)),
                           ResponseAsImage = isContract && Dispatcher.Query(new HasQueryResponseAsImageQuery { Type = Type }),
                           IsCommand = isContract && Dispatcher.Query(new IsCommandTypeQuery(Type)),
                           Namespace = @namespace,
                           IsNotifyPropertyChanged = true
                   };
        }

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Namespace { get; set; }

            public string Name { get; set; }

            public bool IsCommand { get; set; }

            public bool ResponseAsImage { get; set; }

            public bool ResponseAsArray { get; set; }

            public bool IsNotifyPropertyChanged { get; set; }

            #endregion
        }

        #endregion
    }
}