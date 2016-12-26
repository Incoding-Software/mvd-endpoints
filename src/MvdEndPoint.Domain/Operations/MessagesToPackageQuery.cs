namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class MessagesToPackageQuery : QueryBase<byte[]>
    {
        protected override byte[] ExecuteResult()
        {
            var types = Names.Replace("PublicKeyToken=null,", "PublicKeyToken=null|")
                             .Split("|".ToCharArray())
                             .Select(Type.GetType)
                             .Where(r => r.HasAttribute<ServiceContractAttribute>())
                             .ToList();

            string avrNamespace = types.Select(r => r.FirstOrDefaultAttribute<ServiceContractAttribute>())
                                       .FirstOrDefault()
                                       .With(r => r.Namespace)
                                       .Recovery(() => types.First().Module.Name.Replace(".dll", string.Empty));

            switch (Language)
            {
                case Language.JavaCE:
                    return Dispatcher.Query(new AsAndroidQuery
                                            {
                                                    BaseUrl = BaseUrl,
                                                    Types = types
                                            });
                case Language.ObjectiveC:
                    return Dispatcher.Query(new AsIosQuery()
                                            {
                                                    BaseUrl = BaseUrl,
                                                    Types = types
                                            });
                case Language.Csharp:
                    return Dispatcher.Query(new AsWPQuery()
                                            {
                                                    BaseUrl = BaseUrl,
                                                    Namespace = avrNamespace,
                                                    Types = types
                                            });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class AsIosQuery : QueryBase<byte[]>
        {
            protected override byte[] ExecuteResult()
            {
                var imports = new List<string> { "ModelStateException" };
                var zipQuery = new ToZipQuery();
                foreach (var type in Types)
                {
                    bool hasImage = Dispatcher.Query(new HasQueryResponseAsImageQuery { Type = type });
                    Func<GetNameFromTypeQuery.ModeOf, FileOfIos, string> getFileName = (of, fileOfIos) =>
                                                                                       {
                                                                                           string typeInIos = Dispatcher.Query(new GetNameFromTypeQuery(type))[of];
                                                                                           if (fileOfIos == FileOfIos.H && !hasImage)
                                                                                               imports.Add(typeInIos);
                                                                                           return "{0}.{1}".F(typeInIos, fileOfIos.ToString().ToLower());
                                                                                       };
                    foreach (var ofIos in new[] { FileOfIos.H, FileOfIos.M })
                    {
                        zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Request, ofIos), Dispatcher.Query(new IosRequestCodeGenerateQuery { Type = type, File = ofIos }));
                        if (!hasImage)
                            zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Response, ofIos), Dispatcher.Query(new IosResponseCodeGenerateQuery { Type = type, File = ofIos }));
                    }
                }

                zipQuery.Entries.Add("IncodingHelper.h", Dispatcher.Query(new IosIncodingHelperCodeGenerateQuery { BaseUrl = BaseUrl, File = FileOfIos.H, Imports = imports }));
                zipQuery.Entries.Add("IncodingHelper.m", Dispatcher.Query(new IosIncodingHelperCodeGenerateQuery { BaseUrl = BaseUrl, File = FileOfIos.M }));
                zipQuery.Entries.Add("ModelStateException.h", Dispatcher.Query(new IosModelStateExceptionCodeGenerateQuery { File = FileOfIos.H }));
                zipQuery.Entries.Add("ModelStateException.m", Dispatcher.Query(new IosModelStateExceptionCodeGenerateQuery { File = FileOfIos.M }));

                return Dispatcher.Query(zipQuery);
            }

            #region Properties

            public List<Type> Types { get; set; }

            public string BaseUrl { get; set; }

            #endregion
        }

        public class AsAndroidQuery : QueryBase<byte[]>
        {
            protected override byte[] ExecuteResult()
            {
                string avrNamespace = "Incoding";

                var zipQuery = new ToZipQuery();
                zipQuery.Entries.Add("Incoding/IncodingHelper.java", Dispatcher.Query(new AndroidIncodingHelperCodeGenerateQuery { Namespace = avrNamespace, BaseUrl = BaseUrl }));
                zipQuery.Entries.Add("Incoding/ModelStateException.java", Dispatcher.Query(new AndroidModelStateExceptionCodeGenerateQuery { Namespace = avrNamespace }));
                zipQuery.Entries.Add("Incoding/JsonModelStateData.java", Dispatcher.Query(new AndroidJsonModelStateDataCodeGenerateQuery { Namespace = avrNamespace }));
                foreach (var type in Types)
                {
                    var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = type });
                    Func<GetNameFromTypeQuery.ModeOf, string> getFileName = of => "Incoding/{0}.java".F(Dispatcher.Query(new GetNameFromTypeQuery(type))[of]);
                    zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Listener), Dispatcher.Query(new AndroidListenerCodeGeneratorQuery { Type = type }));
                    zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Request), Dispatcher.Query(new AndroidRequestCodeGenerateQuery { Type = type }));
                    zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Response), Dispatcher.Query(new AndroidResponseCodeGenerateQuery { Type = type }));
                    const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

                    var allProperties = type.GetProperties(bindingFlags).ToList();
                    if (!meta.IsCommand)
                    {
                        var responseType = type.BaseType.GetGenericArguments()[0];
                        responseType = responseType.IsImplement<IEnumerable>() ? responseType.GetGenericArguments()[0] : responseType;
                        allProperties.AddRange(responseType.GetProperties(bindingFlags));
                    }

                    foreach (var enumAsType in allProperties.Where(r => r.PropertyType.IsEnum)
                                                            .Select(r => r.PropertyType))
                    {
                        string enumAsFileName = "Incoding/{0}.java".F(Dispatcher.Query(new GetNameFromTypeQuery(enumAsType))[GetNameFromTypeQuery.ModeOf.Enum]);
                        zipQuery.Entries.Add(enumAsFileName, Dispatcher.Query(new AndroidEnumCodeGenerateQuery
                                                                              {
                                                                                      Type = enumAsType,
                                                                                      Namespace = avrNamespace
                                                                              }));
                    }
                }

                return Dispatcher.Query(zipQuery);
            }

            #region Properties

            public List<Type> Types { get; set; }

            public string BaseUrl { get; set; }

            #endregion
        }

        public class AsWPQuery : QueryBase<byte[]>
        {
            public string BaseUrl { get; set; }

            public string Namespace { get; set; }

            public List<Type> Types { get; set; }

            protected override byte[] ExecuteResult()
            {
                var zipQuery = new ToZipQuery();
                zipQuery.Entries.Add("HttpMessageBase.cs", Dispatcher.Query(new WPGenerateHttpMessageQuery()
                                                                            {
                                                                                    Namespace = Namespace
                                                                            }));

                var shareTypes = new List<Type>();
                foreach (var type in Types)
                {
                    var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = type });
                    var fileName = "{0}.cs".F(meta.Name);
                    if (meta.IsCommand)
                        zipQuery.Entries.Add(fileName, Dispatcher.Query(new WPGenerateCommandQuery() { Type = type }));
                    else
                        zipQuery.Entries.Add(fileName, Dispatcher.Query(new WPGenerateQueryQuery() { Type = type }));

                    if (!meta.IsCommand)
                        shareTypes.AddRange(Dispatcher.Query(new GetShareTypeFromTypeQuery() { Type = type.BaseType.GetGenericArguments()[0] }));
                }

                foreach (var shareType in shareTypes.Distinct())
                    zipQuery.Entries.Add("{0}.cs".F(shareType.Name), Dispatcher.Query(new WPGenerateCommonFileQuery() { Type = shareType }));

                return Dispatcher.Query(zipQuery);
            }
        }

        #region Properties

        public Language Language { get; set; }

        public string Names { get; set; }

        public string BaseUrl { get; set; }

        #endregion
    }
}