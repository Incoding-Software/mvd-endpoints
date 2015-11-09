namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class GetNameFromTypeQuery : QueryBase<Dictionary<GetNameFromTypeQuery.ModeOf, string>>
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
        public GetNameFromTypeQuery() { }

        public GetNameFromTypeQuery(Type type)
        {
            Type = type;
        }

        #endregion

        #region Properties

        public Type Type { get; set; }

        #endregion

        #region Enums

        public enum ModeOf
        {
            Request, 

            Response, 

            Task, 

            Listener, 

            Enum, 

            Nested
        }

        #endregion

        protected override Dictionary<ModeOf, string> ExecuteResult()
        {
            return new Dictionary<ModeOf, string>()
                   {
                           { ModeOf.Request, Type.Name + "Request" }, 
                           { ModeOf.Response, Type.Name + "Response" }, 
                           { ModeOf.Task, Type.Name + "Task" }, 
                           { ModeOf.Listener, "I" + Type.Name + "Listener" }, 
                           { ModeOf.Enum, Type.FullName.Replace(Type.Namespace + ".", string.Empty).Replace("+", "_") }, 
                           { ModeOf.Nested, Type.Name }, 
                   };
        }
    }
}