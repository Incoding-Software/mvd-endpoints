namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections;
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
            var properties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(r => !r.PropertyType.IsNested && (!r.PropertyType.IsPrimitive() || r.PropertyType.IsEnum))
                                 .ToList();
            var res = new List<Type>();
            foreach (var type in properties.Select(s => s.PropertyType))
            {
                if (type.IsImplement<IEnumerable>())
                {
                    var typeArgument = type.GenericTypeArguments[0];
                    res.Add(typeArgument);
                    res.AddRange(Dispatcher.Query(new GetShareTypeFromTypeQuery()
                                                  {
                                                          Type = typeArgument
                                                  }));
                }
                else
                {
                    res.Add(type);
                }
            }

            return res;
        }
    }
}