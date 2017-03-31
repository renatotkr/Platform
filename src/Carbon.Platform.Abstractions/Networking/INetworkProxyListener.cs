using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface INetworkProxyListener
    {
        long Id { get; }

        long ProxyId { get; }

        ushort Port { get; }

        NetworkProtocal Protocal { get; }

        long? CertificateId { get; }
    }
}