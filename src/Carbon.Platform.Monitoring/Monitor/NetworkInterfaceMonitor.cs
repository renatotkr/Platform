using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{
    internal class NetworkInterfaceMonitor : IMonitor
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

        public IEnumerable<MetricData> Observe()
        {
            var next = nic.GetIPStatistics();
            var now = DateTime.UtcNow;

            yield return new MetricData(KnownMetrics.NetworkSentBytes,       tags, next.BytesSent - last.BytesSent,                                 now);
            yield return new MetricData(KnownMetrics.NetworkReceivedBytes,   tags, next.BytesReceived - last.BytesReceived,                         now);
            yield return new MetricData(KnownMetrics.NetworkReceivedPackets, tags, next.NonUnicastPacketsReceived - last.NonUnicastPacketsReceived, now);
            yield return new MetricData(KnownMetrics.NetworkSentPackets,     tags, next.NonUnicastPacketsSent - last.NonUnicastPacketsSent,         now);
            yield return new MetricData(KnownMetrics.NetworkDroppedPackets,  tags, next.IncomingPacketsDiscarded - last.IncomingPacketsDiscarded,   now);

            last = next;
        }

        public void Dispose()
        {
            
        }
    }
}