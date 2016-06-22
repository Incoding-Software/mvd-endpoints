namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Linq;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class DownloadSourceCodeQuery : QueryBase<byte[]>
    {
        public string Id { get; set; }

        public DeviceOfType Device { get; set; }

        public string Url { get; set; }

        protected override byte[] ExecuteResult()
        {
            var endpoint = Repository.GetById<Message>(Id);
            var instanceType = Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = endpoint.Type });
            return Dispatcher.Query(new MessagesToPackageQuery.AsAndroidQuery()
                                    {                
                                            BaseUrl = Url,
                                            Types = new[] {instanceType}.ToList()
                                    });
        }
    }
}