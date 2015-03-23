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

            Listener,

            Enum,

            Nested
        }

        #endregion

        protected override string ExecuteResult()
        {
            switch (Mode)
            {
                case ModeOf.Request:
                    return Type.Name + "Request";
                case ModeOf.Response:
                    return Type.Name + "Response";
                case ModeOf.Task:
                    return Type.Name + "Task";
                case ModeOf.Listener:
                    return "I" + Type.Name + "Listener";
                case ModeOf.Enum:
                    return Type.FullName.Replace(Type.Namespace + ".", "").Replace("+", "_");
                case ModeOf.Nested:
                    return Type.Name;
                default:
                    throw new ArgumentOutOfRangeException("modeOf", "Can't resolve name for type {0}".F(Type.Name));
            }
        }
    }
}