using System;
using System.Diagnostics;

namespace Carbon.Platform.Monitoring
{
    public static class PlatformReport
    {
        public static HostReport Generate(HostObserveration o1, HostObserveration o2)
        {
            #region Preconditions

            if (o1 == null) throw new ArgumentNullException(nameof(o1));
            if (o2 == null) throw new ArgumentNullException(nameof(o2));

            #endregion

            var report = new HostReport {
                HostId            = o1.HostId,
                Apps              = new AppStats[o1.Apps.Length],
                NetworkInterfaces = new NetworkInterfaceStats[o1.NetworkInterfaces.Length],
                Volumes           = new VolumeStats[o1.Volumes.Length],
                Memory            = o2.Memory,
                Processors        = new ProcessorStats[o1.Processors.Length],
                Period            = ReportPeriod.Create(o1.Timestamp, o2.Timestamp)
            };           

            for (int i = 0; i < o1.NetworkInterfaces.Length; i++)
            {
                report.NetworkInterfaces[i] = Generate(o1.NetworkInterfaces[i], o2.NetworkInterfaces[i]);
            }

            for (int i = 0; i < o1.Processors.Length; i++)
            {
                report.Processors[i] = Generate(o1.Processors[i], o2.Processors[i]);
            }

            for (int i = 0; i < o1.Volumes.Length; i++)
            {
                report.Volumes[i] = Generate(o1.Volumes[i], o2.Volumes[i]);
            }

            return report;
        }


        public static AppStats Generate(AppObservation o1, AppObservation o2)
        {
            var duration = o2.Date - o1.Date;

            var errorRate = CounterSample.Calculate(o1.ErrorRateSample, o2.ErrorRateSample);

            return new AppStats {
                AppId        = o1.AppId,
                RequestCount = o2.TotalRequestsSample.RawValue - o1.TotalRequestsSample.RawValue,
                ErrorCount   = (long)(duration.TotalSeconds * errorRate)
            };
        }

        public static NetworkInterfaceStats Generate(NetworkInterfaceObservation o1, NetworkInterfaceObservation o2)
        {
            var duration = o2.Date - o1.Date;

            var bytesReceived      = o2.Stats.BytesReceived             - o1.Stats.BytesReceived;
            var bytesSent          = o2.Stats.BytesSent                 - o1.Stats.BytesSent;
            var packetsDiscarded   = o2.Stats.IncomingPacketsDiscarded  - o1.Stats.IncomingPacketsDiscarded;
            var packetsReceived    = o2.Stats.NonUnicastPacketsReceived - o1.Stats.NonUnicastPacketsReceived;
            var packetsSent        = o2.Stats.NonUnicastPacketsSent     - o1.Stats.NonUnicastPacketsSent;

            packetsReceived  += o2.Stats.UnicastPacketsReceived - o1.Stats.UnicastPacketsReceived;
            packetsSent      += o2.Stats.UnicastPacketsSent - o1.Stats.UnicastPacketsSent;

            /*
            var receiveRate = inCount / duration.TotalSeconds;    // bytes/sec
            var sendRate    = outCount / duration.TotalSeconds;   // bytes/sec
            */

            return new NetworkInterfaceStats {
                BytesReceived    = bytesReceived,
                BytesSent        = bytesSent,
                PacketsDiscarded = packetsDiscarded,
                PacketsReceived  = packetsReceived,
                PacketsSent      = packetsSent
            };
        }

        public static VolumeStats Generate(VolumeObservation o1, VolumeObservation o2)
        {
            var readRate = CounterSample.Calculate(o1.ReadRateSample, o2.ReadRateSample);
            var writeRate = CounterSample.Calculate(o1.WriteRateSample, o2.WriteRateSample);
            var totalSeconds = (o2.Date - o1.Date).TotalSeconds;
            
            var bytesRead    = (long)(readRate * totalSeconds);
            var bytesWritten = (long)(writeRate * totalSeconds);

            return new VolumeStats {
                Available    = o2.Available,
                Size         = o2.Size,
                ReadTime     = CounterSample.Calculate(o1.ReadTimeSample, o2.ReadTimeSample),
                WriteTime    = CounterSample.Calculate(o1.WriteTimeSample, o2.WriteTimeSample),
                BytesRead    = bytesRead,
                BytesWritten = bytesWritten
            };
        }

        public static ProcessorStats Generate(ProcessorObservation o1, ProcessorObservation o2)
        {
            return new ProcessorStats {
              SystemTime = CounterSample.Calculate(o1.SystemTimeSample, o2.SystemTimeSample),
              UserTime   = CounterSample.Calculate(o1.UserTimeSample, o2.UserTimeSample),
            };
        }
    }
}
