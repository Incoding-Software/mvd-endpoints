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
            var session = new Dictionary<string, object>
                              {
                                      { "Type", meta.Name },                                      
                                      {
                                              "Name", Dispatcher.Query(new GetNameFromTypeQuery
                                                                           {
                                                                                   Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Request
                                                                           })
                                      },
                                      { "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type, Device = DeviceOfType.Ios }) },
                                      { "IsGet", !Type.IsImplement<CommandBase>() },
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