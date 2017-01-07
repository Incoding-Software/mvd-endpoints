namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    [MessageExecuteSetting(DataBaseInstance = "Endpoint",IsolationLevel = IsolationLevel.ReadCommitted)]
    public class SyncEndpointCommand : CommandBase
    {
        protected override void Execute()
        {
            var allEndpoints = Dispatcher.Query(new GetEndpointsQuery());
            Dispatcher.Push(new RemoveMissedTypesCommand()
                            {
                                    Endpoints = allEndpoints
                            });
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

                //foreach (var delete in entity.Properties
                //                             .ToList()
                //                             .Where(r => properties.All(s => s.Name != r.Name)))
                //{                    
                //    entity.Properties.Remove(delete);
                //    Repository.Delete(delete);
                //}

                Repository.SaveOrUpdate(entity);

                foreach (var property in properties)
                {
                    var exist = entity.Properties.FirstOrDefault(s => s.Name == property.Name);
                    if (exist != null)
                    {
                        exist.PropertyType = property.PropertyType;
                        exist.GenericType = property.GenericType;
                        exist.Values = property.Values;
                    }
                    else
                    {
                        property.Message = entity;
                        Repository.Save(property);
                    }
                }
            }
        }

        public class RemoveMissedTypesCommand : CommandBase
        {
            public List<GetEndpointsQuery.Response> Endpoints { get; set; }

            protected override void Execute()
            {
                //var propertiesTypes = Endpoints.Select(s => s.Requests).SelectMany(s => s).Select(s => s.Type).ToList();
                //propertiesTypes.AddRange(Endpoints.Select(s => s.Responses).SelectMany(s => s).Select(s => s.Type).ToList());
                //foreach (var delete in Repository.Query<Message.Property>()
                //                                 .ToList()
                //                                 .Where(r => propertiesTypes.Where(s => !s.IsPrimitive()).All(response => response.FullName != r.PropertyType)))
                //    Repository.Delete(delete);

                foreach (var delete in Repository.Query<Message>()
                                                 .ToList()
                                                 .Where(r => Endpoints.All(response => response.Type != r.Type)))
                    Repository.Delete(delete);
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
                    if (Type.IsImplement<IEnumerable>() && Type != typeof(string))
                    {
                        var isArray = Type.GenericTypeArguments.Length == 0;
                        if (isArray && !Type.GetElementType().IsPrimitive())
                            Type = Type.GetElementType();
                        if (!isArray && !Type.GenericTypeArguments[0].IsPrimitive())
                            Type = Type.GenericTypeArguments[0];
                    }

                    var propertyInfos = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(r => (IsWrite ? r.CanWrite : r.CanRead) && !r.HasAttribute<IgnoreDataMemberAttribute>())
                                            .ToList();
                    var res = propertyInfos
                            .Select(r => new Response.Property
                                         {
                                                 Name = r.Name,
                                                 Type = r.PropertyType,
                                                 Values = r.PropertyType.IsEnum ? Dispatcher.Query(new GetEnumForDD()
                                                                                                   {
                                                                                                           TypeId = r.PropertyType.GUID.ToString(),
                                                                                                   })
                                                                                            .Items
                                                                                            .Select(s => s.Text)
                                                                                            .ToList() : new List<string>(),
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

                    public List<string> Values { get; set; }

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