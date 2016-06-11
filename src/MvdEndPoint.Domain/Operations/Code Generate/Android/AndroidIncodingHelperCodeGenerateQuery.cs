namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class AndroidIncodingHelperCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public string Namespace { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var template = new Android_IncodingHelper
                               {
                                       Session = new Dictionary<string, object>
                                                     {
                                                             { "Namespace", Namespace },
                                                             { "Url", BaseUrl.AppendSegment("Dispatcher") },
                                                     }
                               };
            template.Initialize();
            return template.TransformText();
        }
    }
}