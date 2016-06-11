namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;

    #endregion

    public class AndroidListenerCodeGeneratorQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var on = new Android_Listener();
            var response = Dispatcher.Query(new GetMetaFromTypeQuery { Type = Type });
            var names = Dispatcher.Query(new GetNameFromTypeQuery(Type));
            on.Session = new Dictionary<string, object>
                         {
                                 { "Package", response.Package }, 
                                 { "Namespace", response.Namespace }, 
                                 { "Name", names[GetNameFromTypeQuery.ModeOf.Listener] }, 
                                 { "Response", names[GetNameFromTypeQuery.ModeOf.Response] }, 
                                 { "IsArray", Dispatcher.Query(new HasQueryResponseAsArrayQuery(Type)) }
                         };
            on.Initialize();
            return on.TransformText();
        }
    }
}