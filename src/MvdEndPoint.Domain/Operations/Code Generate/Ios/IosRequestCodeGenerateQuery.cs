namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
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

            var names = Dispatcher.Query(new GetNameFromTypeQuery(Type));
            var session = new Dictionary<string, object>
                          {
                                  { "Method", meta.IsCommand ? "Push" : "Query" }, 
                                  { "IsImage", meta.ResponseAsImage }, 
                                  { "Type", meta.Name }, 
                                  { "Response", names[GetNameFromTypeQuery.ModeOf.Response] }, 
                                  { "IsArray", meta.ResponseAsArray }, 
                                  { "Name", names[GetNameFromTypeQuery.ModeOf.Request] }, 
                                  { "Properties", Dispatcher.Query(new GetPropertiesQuery { Type = Type, Device = DeviceOfType.Ios, IsCommand = meta.IsCommand }) }, 
                                  { "IsQuery", !meta.IsCommand }, 
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