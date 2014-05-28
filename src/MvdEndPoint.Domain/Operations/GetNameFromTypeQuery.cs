namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
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

            Listener
        }

        #endregion

        protected override string ExecuteResult()
        {
            if (Type.BaseType.Name.Contains("QueryBase"))
            {
                switch (Mode)
                {
                    case ModeOf.Request:
                        return Type.Name + "Request";
                    case ModeOf.Response:
                        return "{0}_{1}".F(Type.Name, Type.BaseType.GenericTypeArguments[0].Name);
                    case ModeOf.Task:
                        return Type.Name + "Task";
                    case ModeOf.Listener:
                        return "I" + Type.Name + "On";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new ArgumentOutOfRangeException("modeOf", "Can't resolve name for type {0}".F(Type.Name));
        }
    }
}