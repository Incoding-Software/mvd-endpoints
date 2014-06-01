namespace MvdEndPoint.Domain
{
    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;
    using Incoding.MvcContrib;
    using System.Linq;

    public class EnumCodeGenerateQuery : QueryBase<string>
    {
        public Type Type { get; set; }

        protected override string ExecuteResult()
        {
            var template = new Android_Enum();
            template.Session = new Dictionary<string, object>
                                   {
                                           { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Enum }) },
                                           {
                                                   "Values", Type.ToKeyValueVm()
                                                                 .Select(r => r.Value)
                                                                 .ToList()
                                           }
                                   };
            template.Initialize();
            return template.TransformText();
        }
    }
}