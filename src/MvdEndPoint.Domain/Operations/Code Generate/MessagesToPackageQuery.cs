namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class MessagesToPackageQuery : QueryBase<byte[]>
    {
        #region Properties

        public string Names { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override byte[] ExecuteResult()
        {
            var types = Names.Replace("PublicKeyToken=null,", "PublicKeyToken=null|")
                             .Split("|".ToCharArray())
                             .Select(Type.GetType)
                             .Where(r => r.HasAttribute<ServiceContractAttribute>())
                             .ToList();

            var avrNamespace = types.Select(r => r.FirstOrDefaultAttribute<ServiceContractAttribute>())
                                    .First();
            var zipQuery = new ToZipQuery();
            zipQuery.Entries.Add("Incoding/IncodingHelper.java", Dispatcher.Query(new IncodingHelperCodeGenerateQuery { Namespace = avrNamespace.Namespace }));
            zipQuery.Entries.Add("Incoding/ModelStateException.java", Dispatcher.Query(new ModelStateExceptionCodeGenerateQuery { Namespace = avrNamespace.Namespace }));
            zipQuery.Entries.Add("Incoding/JsonModelStateData.java", Dispatcher.Query(new JsonModelStateDataCodeGenerateQuery { Namespace = avrNamespace.Namespace }));
            foreach (var type in types)
            {
                Func<GetNameFromTypeQuery.ModeOf, string> getFileName = of => "{0}/{1}.java".F(type.Name, Dispatcher.Query(new GetNameFromTypeQuery { Type = type, Mode = of, }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Request), Dispatcher.Query(new RequestCodeGenerateQuery { Type = type, BaseUrl = BaseUrl }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Listener), Dispatcher.Query(new ListenerCodeGeneratorQuery { Type = type }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Task), Dispatcher.Query(new TaskCodeGenerateQuery { Type = type }));
                zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Response), Dispatcher.Query(new ResponseCodeGenerateQuery { Type = type }));
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
                    zipQuery.Entries.Add(enumAsFileName, Dispatcher.Query(new EnumCodeGenerateQuery
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