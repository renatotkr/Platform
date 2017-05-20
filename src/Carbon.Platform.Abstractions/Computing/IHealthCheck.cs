using System;

using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IHealthCheck : IManagedResource
    {
        string Host { get; }

        string Path { get; }

        int Port { get; }
        
        NetworkProtocal Protocal { get; }

        TimeSpan Interval { get; }

        TimeSpan Timeout { get; }

        int HealthyThreshold { get; }

        int UnhealthyThreshold { get; }
    }
}


// gcp    | ulong       compute#healthCheck
// fastly |             Healthcheck    
// azure  |             Probe