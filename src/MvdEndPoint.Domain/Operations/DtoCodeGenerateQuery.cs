namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations;
    using System.Linq;
    using Incoding.Extensions;

    #endregion

    public class DtoCodeGenerateQuery : QueryBase<string>
    {
        public string Type { get; set; }

        public string Prefix { get; set; }

        protected override string ExecuteResult()
        {
            var type = System.Type.GetType(Type);

            var dto = new Android_Dto();
            dto.Session = new Dictionary<string, object>
                              {
                                      { "Name", type.Name.Replace("Query", Prefix) },
                                      { "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = type.Name }) }
                              };
            dto.Initialize();
            return dto.TransformText();
        }
    }
}