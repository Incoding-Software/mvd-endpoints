namespace MvdEndPoint.Domain
{
    using System;
    using Incoding.CQRS;

    public class ConvertCSharpTypeToTargetQuery : QueryBase<string>
    {
        public DeviceOfType Device { get; set; }

        public Type Type { get; set; }

        protected override string ExecuteResult()
        {
            return Device == DeviceOfType.Android ? Dispatcher.Query(new ConvertCSharpTypeToJavaQuery { Type = Type })
                           : Dispatcher.Query(new ConvertCSharpTypeToIosQuery { Type = Type });
        }
    }
}