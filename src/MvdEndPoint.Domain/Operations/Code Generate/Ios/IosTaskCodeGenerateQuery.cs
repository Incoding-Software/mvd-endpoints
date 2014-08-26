namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate.Ios;

    #endregion

    public class IosTaskCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public FileOfIos File { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var session = new Dictionary<string, object>
                              {
                                      { "Listener", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) },
                                      { "Request", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Request, Type = Type }) },
                                      { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) },
                                      { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Task, Type = Type }) },
                                      { "HasRequest", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type, Device = DeviceOfType.Ios }).Any() },                                      
                                      { "Type", Type.IsImplement<CommandBase>() ? "Push" : "Query" },
                              };
            switch (File)
            {
                case FileOfIos.H:
                    var taskH = new Ios_Task_h { Session = session };
                    taskH.Initialize();
                    return taskH.TransformText();
                case FileOfIos.M:
                    var taskM = new Ios_Task_m { Session = session };
                    taskM.Initialize();
                    return taskM.TransformText();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}