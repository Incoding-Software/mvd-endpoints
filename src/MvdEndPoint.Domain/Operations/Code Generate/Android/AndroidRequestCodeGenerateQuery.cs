namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate.Android;

    #endregion

    public class AndroidRequestCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var dto = new Android_Request();
            var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });

            dto.Session = new Dictionary<string, object>
                          {
                                  { "Namespace", meta.Namespace }, 
                                  { "Package", meta.Package }, 
                                  { "Type", meta.Name }, 
                                  { "Name", Dispatcher.Query(new GetNameFromTypeQuery(Type))[GetNameFromTypeQuery.ModeOf.Request] }, 
                                  { "Properties", Dispatcher.Query(new GetPropertiesFromTypeQuery { Type = Type, Device = DeviceOfType.Android, IsCommand = meta.IsCommand }) }, 
                                  { "IsGet", !meta.IsCommand }, 
                          };
            dto.Initialize();
            return dto.TransformText();
        }
    }
}