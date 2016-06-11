namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class IosIncodingHelperCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public FileOfIos File { get; set; }

        public string BaseUrl { get; set; }

        public List<string> Imports { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            switch (File)
            {
                case FileOfIos.H:
                    var tmplH = new Ios_IncodingHelper_h
                                    {
                                            Session = new Dictionary<string, object>
                                                          {
                                                                  { "Imports", Imports }
                                                          }
                                    };
                    tmplH.Initialize();
                    return tmplH.TransformText();
                case FileOfIos.M:
                    var tmplM = new Ios_IncodingHelper_m
                                    {
                                            Session = new Dictionary<string, object>
                                                          {
                                                                  { "Url", BaseUrl.AppendSegment("Dispatcher") },
                                                          }
                                    };
                    tmplM.Initialize();
                    return tmplM.TransformText();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}