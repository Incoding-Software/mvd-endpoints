namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetInnerResponseTypesQuery : QueryBase<Dictionary<string, List<GetPropertiesQuery.Response>>>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override Dictionary<string, List<GetPropertiesQuery.Response>> ExecuteResult()
        {
            Func<Type, Type> getType = type =>
                                       {
                                           if (type.IsImplement(typeof(IEnumerable<>)) && !type.IsTypicalType())
                                           {
                                               return type.GetGenericArguments().Length == 1
                                                              ? type.GetGenericArguments()[0]
                                                              : type.GetElementType();
                                           }

                                           return type;
                                       };

            return Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(info => info.CanRead && info.CanWrite)
                       .Where(r =>
                              {
                                  var type = getType(r.PropertyType);
                                  return type != null &&
                                         !type.IsTypicalType() &&
                                         type != typeof(object) &&
                                         type != typeof(MessageExecuteSetting);
                              })
                       .Select(r => getType(r.PropertyType))
                       .Distinct()
                       .ToDictionary(r => r.Name, r => Dispatcher.Query(new GetPropertiesQuery()
                                                                        {
                                                                                Device = DeviceOfType.WP,
                                                                                IsCommand = false,
                                                                                Type = r
                                                                        }));
        }
    }
}