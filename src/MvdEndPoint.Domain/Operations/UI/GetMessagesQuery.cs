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
            var groupBy = Repository.Query(new Message.Order.Default())
                                    .ToList()
                                    .GroupBy(r => r.GroupKey.With(s => s.Id), endpoint => endpoint)
                                    .ToList();
            foreach (var item in groupBy)
            {
                if (!item.Any())
                    continue;

                if (!string.IsNullOrEmpty(item.Key))
                {
                    var group = item.First().GroupKey;
                    res.Add(new Response() { Group = group.Name, IsGroup = true, Id = group.Name.Replace(" ", "_"), EntityId = group.Id, Description = group.Description ?? "Description" });
                }

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
                    res.Add(new Response()
                            {
                                    Id = endpoint.Name.Replace(" ", "_"),
                                    EntityId = endpoint.Id,
                                    Name = endpoint.Name,
                                    IsGroup = false,
                                    Jira = endpoint.Jira.HasValue ? string.Empty : "https://incoding.atlassian.net/browse/BB-{0}".F(endpoint.Jira),
                                    Description = endpoint.Description ?? "Description",
                                    Verb = isCommand ? "POST" : "GET",
                                    Url = url,
                                    SampleOfAndroid = Dispatcher.Query(new AndroidSampleCodeGenerateQuery() { Instance = Activator.CreateInstance(instanceType) }),
                                    Host = baseUrl,
                                    Result = endpoint.Result,
                                    Group = endpoint.GroupKey.With(s => s.Name),
                                    PropertiesOfResponse = endpoint.Properties.Where(r => r.Type == Message.Property.TypeOf.Response).Select(property => new Response.Item(property)).ToList(),
                                    PropertiesOfRequest = endpoint.Properties.Where(s => s.Type == Message.Property.TypeOf.Request).Select(property => new Response.Item(property)).ToList()
                            });
                }
            }
            return res;
        }

        public class AsNav : QueryBase<List<OptGroupVm>>
        {
            protected override List<OptGroupVm> ExecuteResult()
            {
                return Dispatcher.Query(new GetMessagesQuery())
                                 .GroupBy(r => r.Group)
                                 .ToList()
                                 .Select(r =>
                                         {
                                             var items = r.Where(response => !response.IsGroup)
                                                          .Select(s => new KeyValueVm(s.Id, s.Name)
                                                                       {
                                                                               CssClass = string.IsNullOrWhiteSpace(r.Key) ? s.EntityId : string.Empty
                                                                       });
                                             return new OptGroupVm(r.Key, items);
                                         })
                                 .ToList();
            }
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

            public string EntityId { get; set; }

            public string SampleOfAndroid { get; set; }

            public class Item
            {
                public Item(Message.Property r)
                {
                    var propertyType = new DefaultDispatcher().Query(new GetTypeFromPropertyQuery() { Property = r });
                    Name = r.Name;
                    Id = r.Id;
                    Childrens = r.Childs.Select(property => new Item(property)).ToList();
                    Type = propertyType.IsGenericType
                                   ? "{0} of {1}".F(propertyType.Name, propertyType.GenericTypeArguments[0].Name)
                                   : propertyType.Name;
                    Description = r.Description ?? "Description";
                    IsRequired = r.IsRequired;
                    Default = r.Default ?? (propertyType.IsValueType ? Activator.CreateInstance(propertyType).With(s => s.ToString().Recovery("null")) : "null");
                }

                public string Name { get; set; }

                public string Default { get; set; }

                public string Description { get; set; }

                public string Type { get; set; }

                public bool IsRequired { get; set; }

                public string Id { get; set; }

                public List<Item> Childrens { get; set; }
            }
        }
    }
}