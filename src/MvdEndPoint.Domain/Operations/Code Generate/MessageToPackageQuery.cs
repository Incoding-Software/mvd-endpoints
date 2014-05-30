namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.IO;
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

            Func<GetNameFromTypeQuery.ModeOf, string> getFileName = of => Dispatcher.Query(new GetNameFromTypeQuery{Type = type,Mode = of,}) + ".java";

            using (var zip = new ZipFile())
            {
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Request), Dispatcher.Query(new RequestCodeGenerateQuery { Type = type }));
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Response), Dispatcher.Query(new ResponseCodeGenerateQuery { Type = type }));
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Listener), Dispatcher.Query(new ListenerCodeGeneratorQuery { Type = type, }));
                zip.AddEntry(getFileName(GetNameFromTypeQuery.ModeOf.Task), Dispatcher.Query(new TaskCodeGenerateQuery { Type = type, BaseUrl = BaseUrl }));
                using (var stream = new MemoryStream())
                {
                    zip.Save(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}