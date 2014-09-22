namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class MessageToPackageAsIosQuery : QueryBase<byte[]>
    {
        #region Properties

        public List<Type> Types { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override byte[] ExecuteResult()
        {
            var imports = new List<string> { "ModelStateException" };
            var zipQuery = new ToZipQuery();
            foreach (var type in Types)
            {
                var hasImage = Dispatcher.Query(new HasQueryResponseAsImageQuery { Type = type });
                Func<GetNameFromTypeQuery.ModeOf, FileOfIos, string> getFileName = (of, fileOfIos) =>
                                                                                       {
                                                                                           string typeInIos = Dispatcher.Query(new GetNameFromTypeQuery { Type = type, Mode = of, });
                                                                                           if (fileOfIos == FileOfIos.H && !hasImage)
                                                                                               imports.Add(typeInIos);
                                                                                           return "{0}.{1}".F(typeInIos, fileOfIos.ToString().ToLower());
                                                                                       };
                foreach (var ofIos in new[] { FileOfIos.H, FileOfIos.M })
                {
                    zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Request, ofIos), Dispatcher.Query(new IosRequestCodeGenerateQuery { Type = type, File = ofIos }));
                    if (!hasImage)
                        zipQuery.Entries.Add(getFileName(GetNameFromTypeQuery.ModeOf.Response, ofIos), Dispatcher.Query(new IosResponseCodeGenerateQuery { Type = type, File = ofIos }));

                    //const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
                    //foreach (var enumAsType in  type.GetProperties(bindingFlags).ToList()
                    //                                .Where(r => r.PropertyType.IsEnum)
                    //                                .Select(r => r.PropertyType))
                    //{
                    //    string enumAsFileName = "{0}.{1}".F(Dispatcher.Query(new GetNameFromTypeQuery { Type = enumAsType, Mode = GetNameFromTypeQuery.ModeOf.Enum }),ofIos.ToString().ToLower());
                    //    zipQuery.Entries.Add(enumAsFileName, Dispatcher.Query(new IosEnumCodeGenerateQuery
                    //                                                              {
                    //                                                                      Type = enumAsType,
                    //                                                                      File = ofIos
                    //                                                              }));
                    //}

                }
            }
            zipQuery.Entries.Add("IncodingHelper.h", Dispatcher.Query(new IosIncodingHelperCodeGenerateQuery { BaseUrl = BaseUrl, File = FileOfIos.H, Imports = imports }));
            zipQuery.Entries.Add("IncodingHelper.m", Dispatcher.Query(new IosIncodingHelperCodeGenerateQuery { BaseUrl = BaseUrl, File = FileOfIos.M }));
            zipQuery.Entries.Add("ModelStateException.h", Dispatcher.Query(new IosModelStateExceptionCodeGenerateQuery { File = FileOfIos.H }));
            zipQuery.Entries.Add("ModelStateException.m", Dispatcher.Query(new IosModelStateExceptionCodeGenerateQuery { File = FileOfIos.M }));

            return Dispatcher.Query(zipQuery);
        }
    }
}