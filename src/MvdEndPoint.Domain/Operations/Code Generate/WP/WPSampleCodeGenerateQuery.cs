namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.WP;

    #endregion

    public class WPSampleCodeGenerateQuery : QueryBase<string>
    {
        public object Instance { get; set; }

        protected override string ExecuteResult()
        {
            var type = Instance.GetType();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = type });
            var tmpl = new WP_Sample()
                       {
                               Session = new Dictionary<string, object>
                                         {
                                                 { "Name", meta.Name },
                                                 { "Namespace", meta.Namespace },
                                                 { "Properties", Dispatcher.Query(new GetPropertiesQuery { Type = type, Device = DeviceOfType.WP, IsCommand = meta.IsCommand }) },
                                         }
                       };
            tmpl.Initialize();
            return tmpl.TransformText();
        }
    }
}