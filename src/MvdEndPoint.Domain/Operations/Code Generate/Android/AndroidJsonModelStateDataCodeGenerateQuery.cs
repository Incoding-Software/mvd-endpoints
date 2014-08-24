namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    
    #endregion

    public class AndroidJsonModelStateDataCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public string Namespace { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var androidClass = new Android_JsonModelStateData();
            androidClass.Session = new Dictionary<string, object>
                                       {
                                               { "Namespace", Namespace },
                                       };
            androidClass.Initialize();
            return androidClass.TransformText();
        }
    }
}