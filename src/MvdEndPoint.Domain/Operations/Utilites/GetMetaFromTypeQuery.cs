namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class GetMetaFromTypeQuery : QueryBase<GetMetaFromTypeQuery.Response>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Package { get; set; }

            public string Namespace { get; set; }

            public string Name { get; set; }
            
            public bool IsCommand { get; set; }

            public bool ResponseAsImage { get; set; }

            public bool ResponseAsArray { get; set; }

            #endregion
        }

        #endregion

        protected override Response ExecuteResult()
        {
            var serviceContract = Type.FirstOrDefaultAttribute<ServiceContractAttribute>();
            var isContract = serviceContract != null;
            string @namespace = serviceContract.With(r => r.Namespace);
            if (string.IsNullOrWhiteSpace(@namespace))
            {
                string defNamespace = Type.Module.Name.Replace(".dll", string.Empty);
                @namespace = defNamespace;
            }

            return new Response
                   {
                           Package = "{0}.{1}".F(@namespace, Type.Name),
                           Name = Type.Name,
                           ResponseAsArray = isContract && Dispatcher.Query(new HasQueryResponseAsArrayQuery(Type)),
                           ResponseAsImage = isContract && Dispatcher.Query(new HasQueryResponseAsImageQuery { Type = Type }),
                           IsCommand = isContract && Dispatcher.Query(new IsCommandTypeQuery(Type)),
                           Namespace = @namespace
                   };
        }
    }
}