﻿namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class ListenerCodeGeneratorQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var on = new Android_Listener();
            on.Session = new Dictionary<string, object>
                             {
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