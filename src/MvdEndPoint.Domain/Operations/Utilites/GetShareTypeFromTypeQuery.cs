namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetShareTypeFromTypeQuery : QueryBase<List<Type>>
    {
        public Type Type { get; set; }

        protected override List<Type> ExecuteResult()
        {
            var propertyInfos = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return propertyInfos
                       .Where(r => !r.PropertyType.IsNested && (!r.PropertyType.IsTypicalType() || r.PropertyType.IsEnum))
                       .Select(r => r.PropertyType)
                       .ToList();
        }
    }
}