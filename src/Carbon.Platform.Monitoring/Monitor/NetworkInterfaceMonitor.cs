﻿using System;
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

            // network interfaceId=1 sent`bytes=100,recieved`bytes=100,packets`recieved=100


            var result = new[] {
                new MetricData(MetricNames.NetworkSentBytes.Name,       tags, current.BytesSent - last.BytesSent,                                 ts),
                new MetricData(MetricNames.NetworkReceivedBytes.Name,   tags, current.BytesReceived - last.BytesReceived,                         ts),
                new MetricData(MetricNames.NetworkReceivedPackets.Name, tags, current.NonUnicastPacketsReceived - last.NonUnicastPacketsReceived, ts),
                new MetricData(MetricNames.NetworkSentPackets.Name,     tags, current.NonUnicastPacketsSent - last.NonUnicastPacketsSent,         ts),
                new MetricData(MetricNames.NetworkDroppedPackets.Name,  tags, current.IncomingPacketsDiscarded - last.IncomingPacketsDiscarded,   ts),
            };

            last = current;

            return result;
        }
    }
}