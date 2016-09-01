namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetPropertiesQuery : QueryBase<List<GetPropertiesQuery.Response>>
    {
        protected override List<Response> ExecuteResult()
        {
            return (Type.IsImplement<IEnumerable>() ? Type.GetGenericArguments()[0] : Type)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(r => !r.HasAttribute<IgnoreDataMemberAttribute>())
                    .Where(r => r.CanWrite || !IsCommand)
                    .Select(r =>
                            {
                                bool isArray = r.PropertyType != typeof(string) && r.PropertyType.IsImplement<IEnumerable>();
                                var type = r.PropertyType;
                                if (isArray)
                                    type = r.PropertyType.GetElementType() ?? r.PropertyType.GenericTypeArguments[0];
                                var response = new Response
                                               {
                                                       Name = r.Name,                                                       
                                                       Target = type,
                                                       Type = Dispatcher.Query(new ConvertCSharpTypeToTargetQuery()
                                                                               {
                                                                                       Language = Language,
                                                                                       Type = type
                                                                               })
                                               };
                                if (r.PropertyType.IsAnyEquals(typeof(bool), typeof(bool?)))
                                    response.Attributes |= Response.OfAttributes.IsBool;
                                if (r.PropertyType.IsEnum)
                                    response.Attributes |= Response.OfAttributes.IsEnum;
                                if (r.PropertyType.IsAnyEquals(typeof(string), typeof(DateTime), typeof(DateTime?)) || !(ReflectionExtensions.IsPrimitive(r.PropertyType) || r.PropertyType.IsEnum))
                                    response.Attributes |= Response.OfAttributes.IsCanNull;
                                if (r.PropertyType.IsAnyEquals(typeof(DateTime), typeof(DateTime?)))
                                    response.Attributes |= Response.OfAttributes.IsDateTime;
                                if (isArray)
                                    response.Attributes |= Response.OfAttributes.IsArray;
                                if (!type.IsPrimitive())    
                                    response.Attributes |= Response.OfAttributes.IsClass;

                                return response;
                            })
                    .ToList();
        }

        #region Nested classes

        public class Response
        {
            [Flags]
            public enum OfAttributes
            {
                IsEnum = 1,

                IsDateTime = 2,

                IsCanNull = 4,

                IsArray = 8,

                IsBool = 16,

                IsClass = 32
            }

            #region Properties

            public string Name { get; set; }

            public string Type { get; set; }

            public OfAttributes Attributes { get; set; }

            public Type Target { get; set; }

            #endregion
        }

        #endregion

        #region Properties

        public Type Type { get; set; }

        public Language Language { get; set; }

        public bool IsCommand { get; set; }

        #endregion
    }
}