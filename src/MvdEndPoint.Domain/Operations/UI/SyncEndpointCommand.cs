﻿namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class SyncEndpointCommand : CommandBase
    {
        protected override void Execute()
        {
            var allEndpoints = Dispatcher.Query(new GetEndpointsQuery());
            foreach (var delete in Repository.Query<Message>()
                                             .Select(r => new { Id = r.Id, Type = r.Type })
                                             .ToList()
                                             .Where(r => allEndpoints.All(response => response.Type != r.Type)))
                Repository.Delete<Message>(delete.Id);

            foreach (var endpoint in allEndpoints)
            {
                var entity = Repository.Query(whereSpecification: new Message.Where.ByFullName(endpoint.Type))
                                       .FirstOrDefault() ?? new Message()
                                                            {
                                                                    Type = endpoint.Type,
                                                                    Name = endpoint.Name
                                                            };

                
                var properties = new List<Message.Property>(endpoint.Requests.Count + endpoint.Responses.Count);
                properties.AddRange(endpoint.Requests.Select(property => new Message.Property(property, Message.Property.TypeOf.Request)));
                properties.AddRange(endpoint.Responses.Select(property => new Message.Property(property, Message.Property.TypeOf.Response)));
                foreach (var property in properties)
                {
                    var exist = entity.Properties.FirstOrDefault(s => s.Name == property.Name && s.Type == property.Type);
                    if (exist != null)
                    {
                        exist.PropertyType = property.PropertyType;
                        exist.GenericType = property.GenericType;
                    }
                    else
                        entity.Properties.Add(property);
                }

                Repository.SaveOrUpdate(entity);
            }
        }

        public class GetEndpointsQuery : QueryBase<List<GetEndpointsQuery.Response>>
        {
            protected override List<Response> ExecuteResult()
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                                .Select(assembly => assembly.GetLoadableTypes())
                                .SelectMany(s => s)
                                .Where(r => r.HasAttribute<ServiceContractAttribute>() && r.IsImplement<MessageBase>())
                                .Select(instanceType =>
                                        {
                                            var response = new List<Response.Property>();
                                            if (!Dispatcher.Query(new IsCommandTypeQuery(instanceType)))
                                            {
                                                var responseType = instanceType.BaseType.GenericTypeArguments[0];
                                                var isArray = !responseType.IsAnyEquals(typeof(string), typeof(byte[])) && responseType.IsImplement<IEnumerable>();
                                                if (isArray)
                                                    responseType = responseType.GenericTypeArguments[0];

                                                response.AddRange(Dispatcher.Query(new GetPropertiesFromTypeQuery() { Type = responseType, IsWrite = false }));
                                            }

                                            return new Response
                                                   {
                                                           Responses = response,
                                                           Name = instanceType.Name,
                                                           Type = instanceType.FullName,
                                                           Requests = Dispatcher.Query(new GetPropertiesFromTypeQuery() { Type = instanceType, IsWrite = true })
                                                   };
                                        })
                                .ToList();
            }

            public class GetPropertiesFromTypeQuery : QueryBase<List<Response.Property>>
            {
                public Type Type { get; set; }

                public bool IsWrite { get; set; }

                protected override List<Response.Property> ExecuteResult()
                {
                    var propertyInfos = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(r => (IsWrite ? r.CanWrite : r.CanRead) && !r.HasAttribute<IgnoreDataMemberAttribute>())
                                            .ToList();
                    var res = propertyInfos
                            .Select(r => new Response.Property
                                         {
                                                 Name = r.Name,
                                                 Type = r.PropertyType,
                                                 Childrens = r.PropertyType.IsPrimitive()
                                                                     ? new List<Response.Property>()
                                                                     : Dispatcher.Query(new GetPropertiesFromTypeQuery() { Type = r.PropertyType, IsWrite = IsWrite })
                                         })
                            .ToList();

                    return res;
                }
            }

            #region Nested classes

            public class Response
            {
                #region Nested classes

                public class Property
                {
                    public Property()
                    {
                        Childrens = new List<Property>();
                    }

                    #region Properties

                    public Type Type { get; set; }

                    public string Name { get; set; }

                    public List<Property> Childrens { get; set; }

                    #endregion
                }

                #endregion

                #region Properties

                public string Type { get; set; }

                public List<Property> Requests { get; set; }

                public List<Property> Responses { get; set; }

                public string Name { get; set; }

                #endregion
            }

            #endregion
        }
    }
}