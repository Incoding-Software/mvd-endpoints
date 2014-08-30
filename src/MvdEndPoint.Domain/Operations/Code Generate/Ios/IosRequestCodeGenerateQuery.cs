namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate.Ios;

    #endregion

    public class IosRequestCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public FileOfIos File { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });
            bool isQuery = !Type.IsImplement<CommandBase>();
            var session = new Dictionary<string, object>
                              {
                                      { "Method", Type.IsImplement<CommandBase>() ? "Push" : "Query" },
                                      { "Type", meta.Name },
                                      { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Response }) },
                                      { "IsArray", Dispatcher.Query(new HasQueryResponseAsArrayQuery { Type = Type }).Value },
                                      { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Request }) },
                                      { "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type, Device = DeviceOfType.Ios }) },
                                      { "IsQuery", isQuery },
                              };

            switch (File)
            {
                case FileOfIos.H:
                    var tmplH = new Ios_Request_h();
                    tmplH.Session = session;
                    tmplH.Initialize();
                    return tmplH.TransformText();
                case FileOfIos.M:
                    var tmplM = new Ios_Request_m();
                    tmplM.Session = session;
                    tmplM.Initialize();
                    return tmplM.TransformText();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}