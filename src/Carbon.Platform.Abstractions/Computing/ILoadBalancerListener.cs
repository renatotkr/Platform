using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface ILoadBalancerListener : IResource
    {
        long LoadBalancerId { get; }

        ApplicationProtocal Protocal { get; }

        ushort Port { get; }

        // long? CertificateId { get; }
    }
}