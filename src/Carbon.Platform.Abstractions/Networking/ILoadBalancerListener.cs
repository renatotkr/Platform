﻿using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface ILoadBalancerListener : IManagedResource
    {
        long LoadBalancerId { get; }

        ApplicationProtocal Protocal { get; }

        ushort Port { get; }

        long? CertificateId { get; }
    }
}