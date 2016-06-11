namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using Incoding.CQRS;

    #endregion

    public class HasQueryResponseAsImageQuery : QueryBase<bool>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override bool ExecuteResult()
        {
            return !Dispatcher.Query(new IsCommandTypeQuery(Type)) && Type.BaseType.GenericTypeArguments[0] == typeof(byte[]);
        }
    }
}