namespace MvdEndPoint.Domain
{
    using Incoding.CQRS;

    public class MessageToPackageQuery : QueryBase<byte[]>
    {
        protected override byte[] ExecuteResult()
        {
            return null;
        }
    }
}