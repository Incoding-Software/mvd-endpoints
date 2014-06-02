namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetNameFromTypeQuery : QueryBase<string>
    {
        #region Properties

        public ModeOf Mode { get; set; }

        public Type Type { get; set; }

        #endregion

        #region Enums

        public enum ModeOf
        {
            Request,

            Response,

            Task,

            Listener,

            Enum
        }

        #endregion

        protected override string ExecuteResult()
        {
            switch (Mode)
            {
                case ModeOf.Request:
                    return Type.Name + "Request";
                case ModeOf.Response:
                    return Type.IsImplement<CommandBase>() ? Type.Name + "Response" : (Type.BaseType.GenericTypeArguments[0].IsImplement<IEnumerable>()
                                                                                               ? Type.Name + Type.BaseType.GenericTypeArguments[0].GenericTypeArguments[0].Name
                                                                                               : Type.Name + Type.BaseType.GenericTypeArguments[0].Name);
                case ModeOf.Task:
                    return Type.Name + "Task";
                case ModeOf.Listener:
                    return "I" + Type.Name + "Listener";
                case ModeOf.Enum:
                    return Type.FullName.Replace(Type.Namespace + ".", "").Replace("+", "_");
                default:
                    throw new ArgumentOutOfRangeException("modeOf", "Can't resolve name for type {0}".F(Type.Name));
            }
        }
    }
}