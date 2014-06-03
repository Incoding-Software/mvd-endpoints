namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MvcContrib;

    #endregion

    public class MessageToPackageQuery : QueryBase<byte[]>
    {
        #region Properties

        public string AssemblyQualifiedName { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override byte[] ExecuteResult()
        {
            var type = Type.GetType(AssemblyQualifiedName);

            Func<GetNameFromTypeQuery.ModeOf, string> getFileName = of => Dispatcher.Query(new GetNameFromTypeQuery { Type = type, Mode = of, }) + ".java";

            var zipQuery = new ToZipQuery();
            zipQuery.Entries.Add("IncodingHelper.java", Dispatcher.Query(new IncodingHelperCodeGenerateQuery()));
            zipQuery.Entries.Add("ModelStateException.java", Dispatcher.Query(new ModelStateExceptionCodeGenerateQuery()));
            zipQuery.Entries.Add("JsonModelStateData.java", Dispatcher.Query(new ClassCodeGenerateQuery() { Type = typeof(IncodingResult.JsonModelStateData) }));
            zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Request), Dispatcher.Query(new RequestCodeGenerateQuery { Type = type, BaseUrl = BaseUrl }));
            zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Listener), Dispatcher.Query(new ListenerCodeGeneratorQuery { Type = type, }));
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
            foreach (var enumAsType in allProperties.Where(r => r.PropertyType.IsEnum)
                                                    .Select(r => r.PropertyType))
            {
                string enumAsFileName = Dispatcher.Query(new GetNameFromTypeQuery { Type = enumAsType, Mode = GetNameFromTypeQuery.ModeOf.Enum, }) + ".java";
                zipQuery.Entries.Add(enumAsFileName, Dispatcher.Query(new EnumCodeGenerateQuery { Type = enumAsType }));
            }

            return Dispatcher.Query(zipQuery);
        }
    }
}