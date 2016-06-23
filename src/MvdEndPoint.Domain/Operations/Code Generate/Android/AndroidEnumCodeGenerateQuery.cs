namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Endpoint.Operations.Code_Generate.Android;

    #endregion

    public class AndroidEnumCodeGenerateQuery : QueryBase<string>
    {
        protected override string ExecuteResult()
        {
            var template = new Android_Enum();
            var allValues = Enum.GetValues(Type).Cast<Enum>()
                                .ToList();
            template.Session = new Dictionary<string, object>
                               {
                                       { "Namespace", Namespace },
                                       { "Name", Dispatcher.Query(new GetNameFromTypeQuery(Type))[GetNameFromTypeQuery.ModeOf.Enum] },
                                       {
                                               "Values", allValues
                                               .Select((r, i) => new Tuple<string, string, bool>(r.ToString(), r.ToString("d"), i == allValues.Count - 1))
                                               .ToList()
                                       }
                               };
            template.Initialize();
            return template.TransformText();
        }

        #region Properties

        public Type Type { get; set; }

        public string Namespace { get; set; }

        #endregion
    }
}