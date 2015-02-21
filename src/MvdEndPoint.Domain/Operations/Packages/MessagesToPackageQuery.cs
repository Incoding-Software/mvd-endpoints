namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.ServiceModel;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class MessagesToPackageQuery : QueryBase<byte[]>
    {
        #region Properties

        public DeviceOfType Device { get; set; }

        public string Names { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        protected override byte[] ExecuteResult()
        {
            var types = Names.Replace("PublicKeyToken=null,", "PublicKeyToken=null|")
                             .Split("|".ToCharArray())
                             .Select(Type.GetType)
                             .Where(r => r.HasAttribute<ServiceContractAttribute>())
                             .ToList();

            switch (Device)
            {
                case DeviceOfType.Android:
                    return Dispatcher.Query(new MessageToPackageAsAndroidQuery
                                                {
                                                        BaseUrl = BaseUrl,
                                                        Types = types
                                                });
                case DeviceOfType.Ios:
                    return Dispatcher.Query(new MessageToPackageAsIosQuery()
                                                {
                                                        BaseUrl = BaseUrl,
                                                        Types = types
                                                });
                    case DeviceOfType.WP:
                    return Dispatcher.Query(new MessageToPackageAsIosQuery()
                    {
                        BaseUrl = BaseUrl,
                        Types = types
                    });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}