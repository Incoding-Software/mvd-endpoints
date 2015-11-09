namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class GetEndpointsQuery : QueryBase<List<GetEndpointsQuery.Response>>
    {
        #region Properties

        public string Id { get; set; }

        public OfType? Type { get; set; }

        #endregion

        #region Nested classes

        public class Tmpl
        {
            #region Properties

            public string DefaultUrl { get; set; }

            public string DownloadLinkName { get; set; }

            public string BaseUrlName { get; set; }

            public string CheckedTypeName { get; set; }

            public string AllId { get; set; }

            public string DeviceName { get; set; }

            #endregion
        }

        public class Response
        {
            #region Properties

            public Guid GUID { get; set; }

            public string Name { get; set; }

            public string Url { get; set; }

            public string Type { get; set; }

            public string AssemblyQualifiedName { get; set; }

            public List<Property> Properties { get; set; }

            public bool IsCommand { get; set; }

            #endregion

            #region Nested classes

            public class Property
            {
                #region Properties

                public string Name { get; set; }

                public string Type { get; set; }

                public bool IsBool { get; set; }

                public bool IsEnum { get; set; }

                public bool IsSimple { get { return !(IsBool || IsEnum); } }

                public string TypeId { get; set; }

                #endregion
            }

            #endregion
        }

        #endregion

        #region Enums

        public enum OfType
        {
            Command,

            Query,

            View,

            Template
        }

        #endregion

        protected override List<Response> ExecuteResult()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                            .Where(r => r.FullName.Contains("Domain"))
                            .SelectMany(r => r.GetLoadableTypes())
                            .Where(r => r.IsImplement<CommandBase>() || r.BaseType.With(s => s.Name).Recovery(string.Empty).Contains("QueryBase"))
                            .Where(r => r.HasAttribute<ServiceContractAttribute>())
                            .Where(r => string.IsNullOrWhiteSpace(Id) || r.GUID == Guid.Parse(Id))
                            .Where(r => !r.IsGenericType)
                            .Select(instanceType =>
                                        {
                                            bool isCommand = instanceType.IsImplement<CommandBase>();
                                            var methodInfo = typeof(UrlDispatcher).GetMethods().FirstOrDefault(r => r.Name == (isCommand ? "Push" : "Query"));
                                            var getUrl = methodInfo.MakeGenericMethod(instanceType).Invoke(new UrlHelper(HttpContext.Current.Request.RequestContext).Dispatcher(), new[] { Activator.CreateInstance(instanceType) });
                                            return new Response
                                                       {
                                                               GUID = instanceType.GUID,
                                                               Name = instanceType.Name,
                                                               Url = isCommand ? getUrl.ToString() : getUrl.GetType().GetMethod("AsJson").Invoke(getUrl, new object[] { }).ToString(),
                                                               IsCommand = isCommand,
                                                               Type = instanceType.IsImplement<CommandBase>() ? OfType.Command.ToLocalization() : OfType.Query.ToLocalization(),
                                                               AssemblyQualifiedName = HttpUtility.UrlEncode(instanceType.AssemblyQualifiedName),
                                                               Properties = instanceType.GetProperties()
                                                                                        .Where(r => !r.Name.IsAnyEqualsIgnoreCase("Result"))
                                                                                        .Select(r => new Response.Property
                                                                                                         {
                                                                                                                 Name = r.Name,
                                                                                                                 Type = r.PropertyType.Name,
                                                                                                                 IsBool = r.PropertyType.IsAnyEquals(typeof(bool), typeof(bool?)),
                                                                                                                 IsEnum = r.PropertyType.IsEnum,
                                                                                                                 TypeId = r.PropertyType.GUID.ToString()
                                                                                                         })
                                                                                        .ToList()
                                                       };
                                        })
                            .Where(r => !Type.HasValue || r.Type == Type.Value.ToLocalization())
                            .ToList();
        }
    }
}