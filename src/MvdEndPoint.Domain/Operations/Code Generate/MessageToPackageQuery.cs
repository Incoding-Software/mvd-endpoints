namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using Ionic.Zip;

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

            using (var zip = new ZipFile())
            {
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Request), Dispatcher.Query(new RequestCodeGenerateQuery { Type = type, BaseUrl = BaseUrl }));
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Response), Dispatcher.Query(new ResponseCodeGenerateQuery { Type = type }));
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Listener), Dispatcher.Query(new ListenerCodeGeneratorQuery { Type = type, }));
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Task), Dispatcher.Query(new TaskCodeGenerateQuery { Type = type }));

                foreach (var enumAsType in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                               .Where(r => r.PropertyType.IsEnum)
                                               .Select(r => r.PropertyType))
                {
                    string enumAsFileName = Dispatcher.Query(new GetNameFromTypeQuery { Type = enumAsType, Mode = GetNameFromTypeQuery.ModeOf.Enum, }) + ".java";
                    zip.AddEntry(enumAsFileName, Dispatcher.Query(new EnumCodeGenerateQuery { Type = enumAsType }));
                }
                using (var stream = new MemoryStream())
                {
                    zip.Save(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}