namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class ListenerCodeGeneratorQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var on = new Android_Listener();
            on.Session = new Dictionary<string, object>
                             {
                                     { "Namespace", Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type }) }, 
                                     { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) }, 
                                     { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) }, 
                             };
            if (!Type.IsImplement<CommandBase>())
                on.Session.Add("IsArray", Type.BaseType.GenericTypeArguments[0].IsImplement<IEnumerable>());
            on.Initialize();
            return on.TransformText();
        }
    }

    public class GetMetaFromTypeQuery : QueryBase<GetMetaFromTypeQuery.Response>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Namespace { get; set; }

            #endregion
        }

        #endregion

        protected override Response ExecuteResult()
        {
            var serviceContract = Type.FirstOrDefaultAttribute<ServiceContractAttribute>();
            return new Response
                       {
                               Namespace = serviceContract.Namespace
                       };
        }
    }
}