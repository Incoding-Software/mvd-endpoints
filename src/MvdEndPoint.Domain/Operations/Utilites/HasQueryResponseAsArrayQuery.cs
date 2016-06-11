namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class HasQueryResponseAsArrayQuery : QueryBase<bool>
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
        public HasQueryResponseAsArrayQuery() { }

        public HasQueryResponseAsArrayQuery(Type type)
        {
            Type = type;
        }

        #endregion

        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override bool ExecuteResult()
        {
            if (Dispatcher.Query(new IsCommandTypeQuery(Type)))
                return false;

            var responseType = Type.BaseType.GenericTypeArguments[0];
            return !responseType.IsAnyEquals(typeof(string), typeof(byte[])) && responseType.IsImplement<IEnumerable>();
        }
    }
}