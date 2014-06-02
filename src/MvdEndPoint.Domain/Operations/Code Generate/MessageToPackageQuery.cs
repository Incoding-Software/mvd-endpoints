namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;

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
            zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Request), Dispatcher.Query(new RequestCodeGenerateQuery { Type = type, BaseUrl = BaseUrl }));
            zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Listener), Dispatcher.Query(new ListenerCodeGeneratorQuery { Type = type, }));
            zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Task), Dispatcher.Query(new TaskCodeGenerateQuery { Type = type }));
            zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Response), Dispatcher.Query(new ResponseCodeGenerateQuery { Type = type }));
            foreach (var enumAsType in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                           .Where(r => r.PropertyType.IsEnum)
                                           .Select(r => r.PropertyType))
            {
                string enumAsFileName = Dispatcher.Query(new GetNameFromTypeQuery { Type = enumAsType, Mode = GetNameFromTypeQuery.ModeOf.Enum, }) + ".java";
                zipQuery.Entries.Add(enumAsFileName, Dispatcher.Query(new EnumCodeGenerateQuery { Type = enumAsType }));
            }
            return Dispatcher.Query(zipQuery);
        }
    }
}