namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate.Ios;

    #endregion

    public class IosResponseCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public FileOfIos File { get; set; }

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            bool isQuery = !Type.IsImplement<CommandBase>();
            var session = new Dictionary<string, object>
                              {
                                      { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Response }) },
                                      { "Properties", new List<GetPropertiesByTypeQuery.Response>() },
                                      {
                                              "MappingJsonMethodByType", new Dictionary<string, string>
                                                                             {                                                                                     
                                                                                     { ConvertCSharpTypeToIosQuery.String, "stringValue" },
                                                                                     { ConvertCSharpTypeToIosQuery.Int, "intValue" },
                                                                                     { ConvertCSharpTypeToIosQuery.Float, "floatValue" },
                                                                                     { ConvertCSharpTypeToIosQuery.Double, "doubleValue" },
                                                                                     { ConvertCSharpTypeToIosQuery.Boolean, "boolValue" },
                                                                             }
                                      },
                                      { "IsQuery", isQuery },
                              };

            if (isQuery)
            {
                var responseType = Type.BaseType.GenericTypeArguments[0];
                session.Add("IsArray", responseType.IsImplement(typeof(IEnumerable<>)));
                session.Set("Properties", Dispatcher.Query(new GetPropertiesByTypeQuery
                                                               {
                                                                       Type = responseType,
                                                                       Device = DeviceOfType.Ios
                                                               }));
            }

            switch (File)
            {
                case FileOfIos.H:
                    var tmplH = new Ios_Response_h();
                    tmplH.Session = session;
                    tmplH.Initialize();
                    return tmplH.TransformText();
                case FileOfIos.M:
                    var tmplM = new Ios_Response_m();
                    tmplM.Session = session;
                    tmplM.Initialize();
                    return tmplM.TransformText();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}