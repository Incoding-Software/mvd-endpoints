namespace Incoding.Endpoint
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
            foreach (var endpoint in Dispatcher.Query(new GetEndpointsQuery()))
            {
                var entity = Repository.Query(whereSpecification: new Message.Where.ByFullName(endpoint.Type))
                                       .FirstOrDefault() ?? new Message()
                                                            {
                                                                    Type = endpoint.Type,
                                                                    Name = endpoint.Name
                                                            };

                Func<Message.Property.TypeOf, GetEndpointsQuery.Response.Property, Message.Property> toProperty = (type, property) => new Message.Property()
                                                                                                                                      {
                                                                                                                                              Name = property.Name,
                                                                                                                                              PropertyType = property.Type.FullName,
                                                                                                                                              GenericType = property.Type.IsGenericType
                                                                                                                                                                    ? property.Type.GenericTypeArguments[0].FullName
                                                                                                                                                                    : string.Empty,
                                                                                                                                              Type = type
                                                                                                                                      };
                var properties = new List<Message.Property>(endpoint.Requests.Count + endpoint.Responses.Count);
                properties.AddRange(endpoint.Requests.Select(property => toProperty(Message.Property.TypeOf.Request, property)));
                properties.AddRange(endpoint.Responses.Select(property => toProperty(Message.Property.TypeOf.Response, property)));
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

                                                response.AddRange(responseType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                                              .Where(r => r.CanRead && !r.HasAttribute<IgnoreDataMemberAttribute>())
                                                                              .Select(r => new Response.Property
                                                                                           {
                                                                                                   Name = r.Name,
                                                                                                   Type = r.PropertyType,
                                                                                           })
                                                                              .ToList());
                                            }

                                            var properties = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                                         .Where(r => r.CanWrite && !r.HasAttribute<IgnoreDataMemberAttribute>())
                                                                         .Select(r => new Response.Property
                                                                                      {
                                                                                              Name = r.Name,
                                                                                              Type = r.PropertyType,
                                                                                      })
                                                                         .ToList();
                                            return new Response
                                                   {
                                                           Responses = response,
                                                           Name = instanceType.Name,
                                                           Type = instanceType.FullName,
                                                           Requests = properties
                                                   };
                                        })
                                .ToList();
            }

            #region Nested classes

            public class Response
            {
                #region Nested classes

                public class Property
                {
                    #region Properties

                    public Type Type { get; set; }

                    public string Name { get; set; }

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