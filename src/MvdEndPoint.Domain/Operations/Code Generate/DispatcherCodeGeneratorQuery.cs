namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class DispatcherCodeGeneratorQuery : QueryBase<string>
    {
        #region Properties

        public List<Type> Types { get; set; }

        #endregion

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Task { get; set; }

            public string Method { get; set; }

            public string Request { get; set; }

            #endregion
        }

        #endregion

        protected override string ExecuteResult()
        {
            var template = new Android_Disaptcher();
            template.Session = new Dictionary<string, object>
                                   {
                                           {
                                                   "Methods", Types.Select(type => new Response
                                                                                       {
                                                                                               Method = Dispatcher.Query(new GetNameFromTypeQuery
                                                                                                                             {
                                                                                                                                     Mode = GetNameFromTypeQuery.ModeOf.Method, 
                                                                                                                                     Type = type
                                                                                                                             }), 
                                                                                               Request = Dispatcher.Query(new GetNameFromTypeQuery
                                                                                                                              {
                                                                                                                                      Mode = GetNameFromTypeQuery.ModeOf.Request, 
                                                                                                                                      Type = type
                                                                                                                              }), 
                                                                                               Task = Dispatcher.Query(new GetNameFromTypeQuery
                                                                                                                           {
                                                                                                                                   Mode = GetNameFromTypeQuery.ModeOf.Task, 
                                                                                                                                   Type = type
                                                                                                                           })
                                                                                       })
                                                                   .ToList()
                                           }
                                   };

            template.Initialize();
            return template.TransformText();
        }
    }
}