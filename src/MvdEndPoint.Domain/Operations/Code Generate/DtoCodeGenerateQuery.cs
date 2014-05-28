namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class DtoCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public string Type { get; set; }

        public string Prefix { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var type = System.Type.GetType(Type);            
            var dto = new Android_Dto();
            dto.Session = new Dictionary<string, object>
                              {
                                      { "Name", type.Name.Replace("Query", Prefix) }, 
                                      { "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = type }) }
                              };
            dto.Initialize();
            return dto.TransformText();
        }
    }
}