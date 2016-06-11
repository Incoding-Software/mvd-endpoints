namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.MvcContrib;

    #endregion

    public class GetEnumForDD : QueryBase<OptGroupVm>
    {
        #region Properties

        public string TypeId { get; set; }

        #endregion

        protected override OptGroupVm ExecuteResult()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(r => r.ExportedTypes)
                            .FirstOrDefault(r => r.GUID == Guid.Parse(TypeId))
                            .ToKeyValueVm()
                            .ToOptGroup();
        }
    }
}