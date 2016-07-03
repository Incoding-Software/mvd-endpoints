namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Linq;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class DownloadSourceCodeQuery : QueryBase<byte[]>
    {
        public string Id { get; set; }

        public DeviceOfType Device { get; set; }

        protected override byte[] ExecuteResult()
        {
            var endpoint = Repository.GetById<Message>(Id);
            var instanceType = Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = endpoint.Type });
            var uri = Dispatcher.Query(new GetUriByTypeQuery() { Type = instanceType });            
            switch (Device)
            {
                case DeviceOfType.Android:
                    return Dispatcher.Query(new MessagesToPackageQuery.AsAndroidQuery()
                                            {
                                                    BaseUrl = "{0}://{1}".F(uri.Scheme, uri.Authority),
                                                    Types = new[] { instanceType }.ToList()
                                            });
                case DeviceOfType.WP:
                    return Dispatcher.Query(new MessagesToPackageQuery.AsWPQuery()
                                            {
                                                    BaseUrl = "{0}://{1}".F(uri.Scheme, uri.Authority),
                                                    Types = new[] { instanceType }.ToList(),
                                                    Namespace = "Incoding"
                                            });
                default:
                    throw new ArgumentOutOfRangeException("Device", "Device not found {0}".F(Device));
            }
        }
    }
}