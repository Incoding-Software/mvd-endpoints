namespace MvdEndPoint.Domain
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

    public class MessageToPackageAsAndroidQuery : QueryBase<byte[]>
    {
        #region Properties

        public List<Type> Types { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override byte[] ExecuteResult()
        {
            string avrNamespace = Types.Select(r => r.FirstOrDefaultAttribute<ServiceContractAttribute>())
                                       .FirstOrDefault()
                                       .With(r => r.Namespace);
            if (string.IsNullOrWhiteSpace(avrNamespace))
            {
                string defNamespace = Types.First().Module.Name.Replace(".dll", "");
                avrNamespace = defNamespace;
            }

            var zipQuery = new ToZipQuery();
            zipQuery.Entries.Add("Incoding/IncodingHelper.java", Dispatcher.Query(new AndroidIncodingHelperCodeGenerateQuery { Namespace = avrNamespace, BaseUrl = BaseUrl }));
            zipQuery.Entries.Add("Incoding/ModelStateException.java", Dispatcher.Query(new AndroidModelStateExceptionCodeGenerateQuery { Namespace = avrNamespace }));
            zipQuery.Entries.Add("Incoding/JsonModelStateData.java", Dispatcher.Query(new AndroidJsonModelStateDataCodeGenerateQuery { Namespace = avrNamespace }));
            foreach (var type in Types)
            {
                Func<GetNameFromTypeQuery.ModeOf, string> getFileName = of => "{0}/{1}.java".F(type.Name, Dispatcher.Query(new GetNameFromTypeQuery { Type = type, Mode = of, }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Request), Dispatcher.Query(new AndroidRequestCodeGenerateQuery { Type = type }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Listener), Dispatcher.Query(new AndroidListenerCodeGeneratorQuery { Type = type }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Task), Dispatcher.Query(new AndroidTaskCodeGenerateQuery { Type = type }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Response), Dispatcher.Query(new AndroidResponseCodeGenerateQuery { Type = type }));
                const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

                var allProperties = type.GetProperties(bindingFlags).ToList();
                if (!type.IsImplement<CommandBase>())
                {
                    var responseType = type.BaseType.GetGenericArguments()[0];
                    responseType = responseType.IsImplement<IEnumerable>() ? responseType.GetGenericArguments()[0] : responseType;
                    allProperties.AddRange(responseType.GetProperties(bindingFlags));
                }

                var meta = Dispatcher.Query(new GetMetaFromTypeQuery { Type = type });
                foreach (var enumAsType in allProperties.Where(r => r.PropertyType.IsEnum)
                                                        .Select(r => r.PropertyType))
                {
                    string enumAsFileName = "{0}/{1}.java".F(type.Name, Dispatcher.Query(new GetNameFromTypeQuery { Type = enumAsType, Mode = GetNameFromTypeQuery.ModeOf.Enum, }));
                    zipQuery.Entries.Add(enumAsFileName, Dispatcher.Query(new AndroidEnumCodeGenerateQuery
                                                                              {
                                                                                      Type = enumAsType,
                                                                                      Package = meta.Package
                                                                              }));
                }
            }

            return Dispatcher.Query(zipQuery);
        }
    }
}