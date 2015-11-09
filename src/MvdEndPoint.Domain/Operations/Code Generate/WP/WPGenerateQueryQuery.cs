namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate.WP;

    #endregion

    public class WPGenerateQueryQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var engine = new WP_Query();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery() { Type = Type });
            engine.Session = new Dictionary<string, object>()
                             {
                                     { "Meta", meta }, 
                                     { "Properties", Dispatcher.Query(new GetPropertiesFromTypeQuery() { Type = Type, Device = DeviceOfType.WP, IsCommand = meta.IsCommand }) }, 
                                     { "Response", Dispatcher.Query(new GetPropertiesFromTypeQuery() { Type = Type.BaseType.GetGenericArguments()[0], Device = DeviceOfType.WP, IsCommand = meta.IsCommand }) }, 
                             };
            engine.Initialize();
            return engine.TransformText();
        }
    }
}