using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using Carbon.Platform.Metrics;
using Carbon.Time;

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

            var timestamp = new Timestamp(DateTimeOffset.UtcNow).Value;

            yield return new MetricData(MetricNames.NetworkSentBytes.Name,       tags, "byte",  next.BytesSent - last.BytesSent,                                 timestamp);
            yield return new MetricData(MetricNames.NetworkReceivedBytes.Name,   tags, "byte",  next.BytesReceived - last.BytesReceived,                         timestamp);
            yield return new MetricData(MetricNames.NetworkReceivedPackets.Name, tags, "count", next.NonUnicastPacketsReceived - last.NonUnicastPacketsReceived, timestamp);
            yield return new MetricData(MetricNames.NetworkSentPackets.Name,     tags, "count", next.NonUnicastPacketsSent - last.NonUnicastPacketsSent,         timestamp);
            yield return new MetricData(MetricNames.NetworkDroppedPackets.Name,  tags, "count", next.IncomingPacketsDiscarded - last.IncomingPacketsDiscarded,   timestamp);

            last = next;
        }

        public void Dispose()
        {
            
        }
    }
}