namespace MvdEndPoint.UI.Controllers
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
    using MvdEndPoint.Domain;

    #endregion

    public class EndPointItem
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

        public class Tmpl
        {
            #region Properties

            public string DefaultUrl { get; set; }

            public string DownloadLinkId { get; set; }

            public string BaseUrlName { get; set; }

            public string CheckedTypeName { get; set; }

            public string AllId { get; set; }

            #endregion
        }

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

        #region Enums

        public enum OfType
        {
            Command, 

            Query, 

            View, 

            Template
        }

        #endregion
    }

    public class DispatcherController : DispatcherControllerBase
    {
        #region Constructors

        public DispatcherController()
                : base(new[]
                           {
                                   typeof(Bootstrapper).Assembly, 
                                   typeof(DispatcherController).Assembly
                           }) { }

        #endregion

        #region Http action

        [HttpGet]
        public ActionResult Endpoint(string id, EndPointItem.OfType? type)
        {
            var enumerable = typeof(BmApp.Domain.Bootstrapper).Assembly.GetTypes()
                                                              .Where(r => r.IsImplement<CommandBase>() || r.BaseType.With(s => s.Name).Recovery(string.Empty).Contains("QueryBase"))
                                                              .Where(r => r.HasAttribute<ServiceContractAttribute>())
                                                              .Where(r => string.IsNullOrWhiteSpace(id) || r.GUID == Guid.Parse(id))
                                                              .Where(r => !r.IsGenericType);
            var endPointItems = enumerable
                    .Select(instanceType =>
                                {
                                    bool isCommand = instanceType.IsImplement<CommandBase>();
                                    var methodInfo = typeof(UrlDispatcher).GetMethods().FirstOrDefault(r => r.Name == (isCommand ? "Push" : "Query"));
                                    var getUrl = methodInfo.MakeGenericMethod(instanceType).Invoke(Url.Dispatcher(), new[] { Activator.CreateInstance(instanceType) });
                                    return new EndPointItem
                                               {
                                                       GUID = instanceType.GUID, 
                                                       Name = instanceType.Name, 
                                                       Url = isCommand ? getUrl.ToString() : getUrl.GetType().GetMethod("AsJson").Invoke(getUrl, new object[] { }).ToString(), 
                                                       IsCommand = isCommand, 
                                                       Type = instanceType.IsImplement<CommandBase>() ? EndPointItem.OfType.Command.ToLocalization() : EndPointItem.OfType.Query.ToLocalization(), 
                                                       AssemblyQualifiedName = HttpUtility.UrlEncode(instanceType.AssemblyQualifiedName), 
                                                       Properties = instanceType.GetProperties()
                                                                                .Where(r => !r.Name.IsAnyEqualsIgnoreCase("Result"))
                                                                                .Select(r => new EndPointItem.Property
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
                    .Where(r => !type.HasValue || r.Type == type.Value.ToLocalization());
            return IncJson(endPointItems);
        }

        [HttpGet]
        public ActionResult FetchEnum(string typeId)
        {
            var type = typeof(Bootstrapper).Assembly
                                           .GetTypes()
                                           .FirstOrDefault(r => r.GUID == Guid.Parse(typeId));

            return IncJson(type.ToKeyValueVm().ToOptGroup());
        }

        #endregion
    }
}