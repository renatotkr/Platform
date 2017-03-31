using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Net;

    [Dataset("NetworkProxyListener")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkProxyListener : INetworkProxyListener, ICloudResource
    {
        // proxyId | index
        [Member("id"), Key]
        public long Id { get; set; }

        // e.g. HTTP | HTTPS
        [Member("protocal")]
        public NetworkProtocal Protocal { get; set; }

        [Member("port")]
        public ushort Port { get; set; }
     
        [Member("sslCertificateId")]
        public long? CertificateId { get; set; }

        // TODO: SSL Policy

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        public long ProxyId => ScopedId.GetScope(Id);

        #region IResource

        // aws
        [Member("providerId")]
        public int ProviderId { get; set; }
        
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        // arn:aws:elasticloadbalancing:us-west-2:123456789012:listener/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2

        ResourceType ICloudResource.Type => ResourceType.NetworkProxyListener;

        #endregion
    }
}