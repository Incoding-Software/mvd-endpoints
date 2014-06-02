namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class ClassCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var androidClass = new Android_Class();
            androidClass.Session = new Dictionary<string, object>
                                       {
                                               { "Name", Type.Name }, 
                                               { "Properties", Dispatcher.Query(new GetPropertiesByTypeQuery { Type = Type }) }
                                       };
            androidClass.Initialize();
            return androidClass.TransformText();
        }
    }
}