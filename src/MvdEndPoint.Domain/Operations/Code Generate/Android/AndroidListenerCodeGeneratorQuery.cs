namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
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
            on.Session = new Dictionary<string, object>
                             {
                                     { "Package", response.Package },
                                     { "Namespace", response.Namespace },
                                     { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Listener, Type = Type }) },
                                     { "Response", Dispatcher.Query(new GetNameFromTypeQuery { Mode = GetNameFromTypeQuery.ModeOf.Response, Type = Type }) },
                             };
            if (!Type.IsImplement<CommandBase>())
                on.Session.Add("IsArray", Type.BaseType.GenericTypeArguments[0].IsImplement<IEnumerable>());
            on.Initialize();
            return on.TransformText();
        }
    }
}