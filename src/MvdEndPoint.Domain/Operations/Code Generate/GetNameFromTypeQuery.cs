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
        }

        #endregion

        protected override string ExecuteResult()
        {
            string res;
            switch (Mode)
            {
                case ModeOf.Request:
                    res = Type.Name + "Request";
                    break;
                case ModeOf.Response:
                    if (Type.BaseType.GenericTypeArguments[0].IsImplement<IEnumerable>())
                        res = Type.Name + Type.BaseType.GenericTypeArguments[0].GenericTypeArguments[0].Name;
                    else
                        res = Type.Name + Type.BaseType.GenericTypeArguments[0].Name;
                    break;
                case ModeOf.Task:
                    res = Type.Name + "Task";
                    break;
                case ModeOf.Listener:
                    res = "I" + Type.Name + "Listener";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("modeOf", "Can't resolve name for type {0}".F(Type.Name));
            }

            return res;
        }
    }
}