namespace Incoding.Endpoint
{ 
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Data;
    
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using MongoDB.Bson.Serialization;
    using NHibernate.Context;
	using NHibernate.Tool.hbm2ddl;
	using StructureMap.Graph;

    public static class Bootstrapper
    {
        public static void Start()
        {
            LoggingFactory.Instance.Initialize(logging =>
                                                   {
                                                       string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                                                       logging.WithPolicy(policy => policy.For(LogType.Debug).Use(FileLogger.WithAtOnceReplace(path, () => "Debug_{0}.txt".F(DateTime.Now.ToString("yyyyMMdd")))));
                                                   });

            IoCFactory.Instance.Initialize(init => init.WithProvider(new StructureMapIoCProvider(registry =>
                                                                                                     {
                                                                                                         registry.For<IDispatcher>().Use<DefaultDispatcher>();                                                                                                         
                                                                                                         registry.For<ITemplateFactory>().Singleton().Use<TemplateHandlebarsFactory>();


                                                                                                         BsonClassMap.RegisterClassMap<IncEntityBase>(map => map.UnmapProperty(r => r.Id));
                                                                                                         registry.For<IMongoDbSessionFactory>().Use(() => new MongoDbSessionFactory("mongodb://localhost:27017/thetracker"));
                                                                                                         registry.For<IUnitOfWorkFactory>().Singleton().Use<MongoDbUnitOfWorkFactory>();
                                                                                                         registry.For<IUnitOfWork>().Singleton().Use<MongoDbUnitOfWork>();
                                                                                                         registry.For<IRepository>().Use<MongoDbRepository>();

                                                                                                         registry.Scan(r =>
                                                                                                                           {
                                                                                                                               r.TheCallingAssembly();
                                                                                                                               r.WithDefaultConventions();

                                                                                                                               r.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));                                                                                                                               
                                                                                                                               r.AddAllTypesOf<ISetUp>();
                                                                                                                           });
                                                                                                     })));

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new IncValidatorFactory()));
            FluentValidationModelValidatorProvider.Configure();
            

			TemplateHandlebarsFactory.GetVersion =() => Guid.NewGuid().ToString();// disable cache template on server side as default

            var ajaxDef = JqueryAjaxOptions.Default;
            ajaxDef.Cache = false; // disabled cache as default
        }
    }

}