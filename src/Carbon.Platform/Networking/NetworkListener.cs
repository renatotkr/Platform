using System;
using Carbon.Data.Annotations;
using Carbon.Platform.Networking;

namespace Carbon.Platform
{
    [Dataset("NetworkListener")]
    public class NetworkListener
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("name")]
        public string Name { get; set; }

        [Member("proxyId")]
        [Indexed]
        public long ProxyId { get; set; }

        [Member("protocal")]
        public NetworkProtocal Protocal { get; set; }

        [Member("port")]
        public int Port { get; set; }
     
        [Member("sslCertificateId")]
        public long? SslCertificateId { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #region IResource

        // ResourceType ICloudResource.Type => ResourceType.Listener;

        #endregion 
    }
}

// arn:aws:elasticloadbalancing:us-west-2:123456789012:listener/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2

// Ciphers