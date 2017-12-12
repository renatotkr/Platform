using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using Carbon.Platform.Metrics;
using Carbon.Time;

namespace Carbon.Platform.Monitoring
{
    internal sealed class NetworkInterfaceMonitor : ResourceMonitor
    {
        private readonly NetworkInterface nic;
        private readonly Dimension[] tags;
        
        public NetworkInterfaceMonitor(Dimension[] dimensions, NetworkInterface nic)
        {
            this.tags = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
            this.nic  = nic        ?? throw new ArgumentNullException(nameof(nic));

            last = nic.GetIPStatistics();
        }

        private IPInterfaceStatistics last;

        public override MetricData[] Observe()
        {
            var current = nic.GetIPStatistics();

            var ts = new Timestamp(DateTime.UtcNow);

            var result = new[] {
                new MetricData(MetricNames.NetworkSentBytes.Name,       tags, "byte",  current.BytesSent - last.BytesSent,                                 ts),
                new MetricData(MetricNames.NetworkReceivedBytes.Name,   tags, "byte",  current.BytesReceived - last.BytesReceived,                         ts),
                new MetricData(MetricNames.NetworkReceivedPackets.Name, tags, "count", current.NonUnicastPacketsReceived - last.NonUnicastPacketsReceived, ts),
                new MetricData(MetricNames.NetworkSentPackets.Name,     tags, "count", current.NonUnicastPacketsSent - last.NonUnicastPacketsSent,         ts),
                new MetricData(MetricNames.NetworkDroppedPackets.Name,  tags, "count", current.IncomingPacketsDiscarded - last.IncomingPacketsDiscarded,   ts),
            };

            last = current;

            return result;
        }
    }
}