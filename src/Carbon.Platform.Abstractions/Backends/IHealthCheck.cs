using System;

using Carbon.Net;

namespace Carbon.Platform.Computing
{
    public interface IHealthCheck
    {
        long Id { get; }

        string Host { get; }

        string Path { get; }

        ushort Port { get; }

        NetworkProtocal Protocal { get; }

        TimeSpan Interval { get; }

        TimeSpan Timeout { get; }

        Threshold HealthyThreshold { get; }

        Threshold UnhealthyThreshold { get; }
    }
}