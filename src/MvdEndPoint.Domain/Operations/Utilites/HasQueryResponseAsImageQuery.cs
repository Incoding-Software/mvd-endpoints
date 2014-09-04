namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class HasQueryResponseAsImageQuery : QueryBase<IncBoolResponse>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override IncBoolResponse ExecuteResult()
        {
            if (Type.IsImplement<CommandBase>())
                return false;

            return Type.BaseType.GenericTypeArguments[0] == typeof(byte[]);
        }
    }
}