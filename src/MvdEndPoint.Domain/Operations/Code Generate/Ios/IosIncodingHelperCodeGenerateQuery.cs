namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using MvdEndPoint.Domain.Operations.Code_Generate.Ios;

    #endregion

    public class IosIncodingHelperCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public FileOfIos File { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var session = new Dictionary<string, object>
                              {
                                      { "Url", BaseUrl.AppendSegment("Dispatcher") },
                              };
            switch (File)
            {
                case FileOfIos.H:
                    var tmplH = new Ios_IncodingHelper_h { Session = session };
                    tmplH.Initialize();
                    return tmplH.TransformText();
                case FileOfIos.M:
                    var tmplM = new Ios_IncodingHelper_m { Session = session };
                    tmplM.Initialize();
                    return tmplM.TransformText();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}