namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class HasQueryResponseAsArrayQuery : QueryBase<IncBoolResponse>
    {
        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override IncBoolResponse ExecuteResult()
        {
            if (Type.IsImplement<CommandBase>())
                return false;

            var responseType = Type.BaseType.GenericTypeArguments[0];
            return !responseType.IsAnyEquals(typeof(string), typeof(byte[])) && responseType.IsImplement<IEnumerable>();
        }
    }
}