namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using Incoding.CQRS;

    #endregion

    public class ConvertCSharpTypeToTargetQuery : QueryBase<string>
    {
        #region Properties

        public DeviceOfType Device { get; set; }

        public Type Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            switch (Device)
            {
                case DeviceOfType.Android:
                    return Dispatcher.Query(new ConvertCSharpTypeToJavaQuery { Type = Type });
                case DeviceOfType.Ios:
                    return Dispatcher.Query(new ConvertCSharpTypeToIosQuery { Type = Type });
                case DeviceOfType.WP:
                    return Type.Name;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}