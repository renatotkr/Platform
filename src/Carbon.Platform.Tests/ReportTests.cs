using System;

using Xunit;

namespace Carbon.Platform.Tests
{
    public class ReportTests
    {
        [Fact]
        public void A()
        {
            long GiB_30 = 1024L * 1024L * 1024L * 30L;
            long GiB_8 = 1024L * 1024L * 1024L * 8L;
            long GiB_5 = 1024L * 1024L * 1024L * 5L;

            var start = new DateTime(2010, 01, 01);
            var end = new DateTime(2011, 01, 01);

            var report = new HostReport {
                HostId = 1,
                Apps = new[] {
                    new AppStats { AppId = 2, ErrorCount = 19, RequestCount = 52_345_234 },
                    new AppStats { AppId = 3, ErrorCount = 19, RequestCount = 52_345_234 }
                },
                Processors = new[] {
                    new ProcessorStats { StealTime = 0f,    SystemTime = 0.1f,    UserTime = 0.5f },
                    new ProcessorStats { StealTime = 0.34f, SystemTime = 0.1934f, UserTime = 0.5f },
                    new ProcessorStats { StealTime = 0f,    SystemTime = 0.1f,    UserTime = 0.5f },
                    new ProcessorStats { StealTime = 0f,    SystemTime = 0.1f,    UserTime = 0.5f }
                },
                Memory = new MemoryStats(used: 1_345_180_005, total: GiB_8),
                NetworkInterfaces = new[] {
                    new NetworkInterfaceStats { BytesReceived = 100, BytesSent = 200, PacketsDiscarded = 0, PacketsReceived = 100, PacketsSent = 300 }
                },
                Volumes = new[] {
                    new VolumeStats { Size = GiB_30, Available = GiB_5, BytesRead = 400, BytesWritten = 1000, WriteTime = 0.05f, ReadTime = 0.048f }
                },
                Period = ReportPeriod.Create(start, end)
            };

            var data = report.Serialize();

            var d = HostReport.Deserialize(data);

            Assert.Equal(1, d.HostId);
            Assert.Equal(4, d.Processors.Length);
            Assert.Equal(1, d.Volumes.Length);
            Assert.Equal(1, d.NetworkInterfaces.Length);
            Assert.Equal(2, d.Apps.Length);


            Assert.Equal(0.34f,   d.Processors[1].StealTime);
            Assert.Equal(0.1934f, d.Processors[1].SystemTime);
            Assert.Equal(0.50f,   d.Processors[1].UserTime);

            Assert.Equal(GiB_30, d.Volumes[0].Size);
            Assert.Equal(GiB_5,  d.Volumes[0].Available);
            Assert.Equal(0.05f,  d.Volumes[0].WriteTime);
            Assert.Equal(0.048f, d.Volumes[0].ReadTime);
            Assert.Equal(GiB_30, d.Volumes[0].Size);

            Assert.Equal(GiB_8, d.Memory.BytesTotal);
            Assert.Equal(1_345_180_005, d.Memory.BytesUsed);

            Assert.Equal(start, d.Period.Start);
            Assert.Equal(end, d.Period.End);

        }
    }
}
