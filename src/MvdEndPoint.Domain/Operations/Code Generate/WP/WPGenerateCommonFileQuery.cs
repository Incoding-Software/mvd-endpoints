namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;

    #endregion

    public class WPGenerateCommonFileQuery : QueryBase<string>
    {
        public Type Type { get; set; }

        protected override string ExecuteResult()
        {
            if (Type.IsClass)
            {
                var engine = new WP_Class();
                var meta = Dispatcher.Query(new GetMetaFromTypeQuery() { Type = Type });
                engine.Session = new Dictionary<string, object>()
                             {
                                     { "Meta", meta },
                                     { "Properties", Dispatcher.Query(new GetPropertiesQuery() { Type = Type, Device = DeviceOfType.WP }) },
                             };
                engine.Initialize();
                return engine.TransformText();
            }
            else
            {
                var engine = new WP_Enum();
                var meta = Dispatcher.Query(new GetMetaFromTypeQuery() { Type = Type });
                engine.Session = new Dictionary<string, object>()
                             {
                                     { "Meta", meta },
                                     { "Values", Dispatcher.Query(new GetValuesOfEnumQuery() { Type = Type }) },
                             };
                engine.Initialize();
                return engine.TransformText();
            }
  
        }
    }
}