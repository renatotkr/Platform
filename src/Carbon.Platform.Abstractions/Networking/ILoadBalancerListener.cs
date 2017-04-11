using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface ILoadBalancerListener : IManagedResource
    {
        long Id { get; }

        long LoadBalancerId { get; }

        ApplicationProtocal Protocal { get; }

        ushort Port { get; }

        long? CertificateId { get; }
    }
}