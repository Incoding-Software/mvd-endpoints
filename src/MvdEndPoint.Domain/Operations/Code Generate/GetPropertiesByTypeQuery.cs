namespace MvdEndPoint.Domain
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

    public class GetPropertiesByTypeQuery : QueryBase<List<GetPropertiesByTypeQuery.Response>>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Name { get; set; }

            public string Type { get; set; }

            public bool IsEnum { get; set; }

            #endregion
        }

        #endregion

        protected override List<Response> ExecuteResult()
        {
            return (Type.IsImplement<IEnumerable>() ? Type.GenericTypeArguments[0] : Type)
                    .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .Where(r => r.CanWrite)
                    .Select(r => new Response
                                     {
                                             Name = r.Name,
                                             Type = Dispatcher.Query(new ConvertCSharpTypeToJavaQuery
                                                                         {
                                                                                 Type = r.PropertyType
                                                                         }),
                                             IsEnum = r.PropertyType.IsEnum
                                     })
                    .ToList();
        }
    }
}