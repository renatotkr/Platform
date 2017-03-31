using System;

using Carbon.Data.Annotations;
using Carbon.Net;

namespace Carbon.Platform.Computing
{
    [Dataset("HealthChecks")]
    public class HealthCheck : IHealthCheck
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("path")]
        public string Path { get; set; }

        [Member("port")]
        public ushort Port { get; set; }

        [Member("host"), Optional]
        public string Host { get; set; }

        [Member("protocal")]
        public NetworkProtocal Protocal { get; set; }
        
        [Member("interval")]
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(30);

        [Member("timeout")]
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
        
        [Member("healthyThreshold")]
        public Threshold HealthyThreshold { get; set; }     // e.g. 4/5

        [Member("unhealtyThreshold")]
        public Threshold UnhealthyThreshold { get; set; }   // e.g. 5.5
    }
}