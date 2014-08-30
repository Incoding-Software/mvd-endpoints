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
            bool isArray = Dispatcher.Query(new HasQueryResponseAsArrayQuery { Type = Type }).Value;
            var session = new Dictionary<string, object>
                              {
                                      { "Name", Dispatcher.Query(new GetNameFromTypeQuery { Type = Type, Mode = GetNameFromTypeQuery.ModeOf.Response }) },
                                      { "Properties", new List<GetPropertiesByTypeQuery.Response>() },
                                      { "IsQuery", isQuery },
                                      { "IsArray", isArray }
                              };

            if (isQuery)
            {
                session.Set("Properties", Dispatcher.Query(new GetPropertiesByTypeQuery
                                                               {
                                                                       Type = Type.BaseType.GenericTypeArguments[0],
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
                    if (isQuery)
                    {
                        if (isArray)
                        {
                            var tmplMAsQueryIsArray = new Ios_Response_Query_As_Array_m();
                            tmplMAsQueryIsArray.Session = session;
                            tmplMAsQueryIsArray.Initialize();
                            return tmplMAsQueryIsArray.TransformText();
                        }
                        else
                        {
                            var tmplMAsQuery = new Ios_Response_Query_m();
                            tmplMAsQuery.Session = session;
                            tmplMAsQuery.Initialize();
                            return tmplMAsQuery.TransformText();
                        }
                    }
                    else
                    {
                        var tmplM = new Ios_Response_m();
                        tmplM.Session = session;
                        tmplM.Initialize();
                        return tmplM.TransformText();
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}