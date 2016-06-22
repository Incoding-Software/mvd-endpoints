namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class GetMessagesQuery : QueryBase<List<GetMessagesQuery.Response>>
    {
        protected override List<Response> ExecuteResult()
        {
            var res = new List<Response>();
            foreach (var item in Repository.Query<Message>()
                                           .ToList()
                                           .GroupBy(r => r.GroupKey.With(s => s.Name), endpoint => endpoint))
            {
                if (!string.IsNullOrWhiteSpace(item.Key))
                    res.Add(new Response() { Group = item.Key, IsGroup = true, Id = item.Key.Replace(" ", "_") });

                foreach (var endpoint in item)
                {
                    var instanceType = Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = endpoint.Type });
                    bool isCommand = Dispatcher.Query(new IsCommandTypeQuery(instanceType));
                    var methodInfo = typeof(UrlDispatcher).GetMethods().FirstOrDefault(r => r.Name == (isCommand ? "Push" : "Query"));
                    var getUrl = methodInfo.MakeGenericMethod(instanceType).Invoke(new UrlHelper(HttpContext.Current.Request.RequestContext).Dispatcher(), new[] { Activator.CreateInstance(instanceType) });

                    var url = isCommand
                                      ? getUrl.ToString()
                                      : getUrl.GetType().GetMethod("AsJson").Invoke(getUrl, new object[] { }).ToString();

                    var baseUrl = "{0}://{1}".F(HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority);
                    Func<Message.Property, Response.Item> toProperty = r =>
                                                                       {
                                                                           var propertyType = Dispatcher.Query(new GetTypeFromPropertyQuery() { Property = r });
                                                                           return new Response.Item()
                                                                                  {
                                                                                          Name = r.Name,
                                                                                          Type = propertyType.IsGenericType
                                                                                                         ? "{0} of {1}".F(propertyType.Name, propertyType.GenericTypeArguments[0].Name)
                                                                                                         : propertyType.Name,
                                                                                          Description = r.Description,
                                                                                          IsRequired = r.IsRequired,
                                                                                          Default = r.Default ?? (propertyType.IsValueType ? Activator.CreateInstance(propertyType).With(s => s.ToString().Recovery("null")) : "null")
                                                                                  };
                                                                       };
                    res.Add(new Response()
                            {
                                    Id = endpoint.Name.Replace(" ", "_"),
                                    EndpointId = endpoint.Id,
                                    Name = endpoint.Name,
                                    Jira = endpoint.Jira.HasValue ? string.Empty : "https://incoding.atlassian.net/browse/BB-{0}".F(endpoint.Jira),
                                    Description = endpoint.Description,
                                    Verb = isCommand ? "POST" : "GET",
                                    Url = url,
                                    SampleOfAndroid = Dispatcher.Query(new AndroidSampleCodeGenerateQuery() { Instance = Activator.CreateInstance(instanceType) }),
                                    Host = baseUrl,
                                    Result = endpoint.Result,
                                    Group = endpoint.GroupKey.With(s => s.Name),
                                    PropertiesOfResponse = endpoint.Properties.Where(r => r.Type == Message.Property.TypeOf.Response).Select(toProperty).ToList(),
                                    PropertiesOfRequest = endpoint.Properties.Where(s => s.Type == Message.Property.TypeOf.Request).Select(toProperty).ToList()
                            });
                }
            }
            return res;
        }

        public class Response
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public List<Item> PropertiesOfRequest { get; set; }

            public string Verb { get; set; }

            public string Url { get; set; }

            public string Jira { get; set; }

            public string Result { get; set; }

            public string Group { get; set; }

            public bool IsGroup { get; set; }

            public List<Item> PropertiesOfResponse { get; set; }

            public string Host { get; set; }

            public string EndpointId { get; set; }

            public string SampleOfAndroid { get; set; }

            public class Item
            {
                public string Name { get; set; }

                public string Default { get; set; }

                public string Description { get; set; }

                public string Type { get; set; }

                public bool IsRequired { get; set; }
            }
        }
    }

    public class GetTypeFromPropertyQuery : QueryBase<Type>
    {
        public Message.Property Property { get; set; }

        protected override Type ExecuteResult()
        {
            var propertyType = Property.PropertyType == "System.Byte[]" ? typeof(byte[]) : Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = Property.PropertyType });
            if (!string.IsNullOrWhiteSpace(Property.GenericType))
                propertyType = propertyType.MakeGenericType(Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = Property.GenericType }));

            return propertyType;
        }
    }
}