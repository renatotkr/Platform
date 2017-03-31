using System;
using System.Diagnostics;
using System.IO;

namespace Carbon.Platform.Monitoring
{
    public class VolumeObserver : IObservable<VolumeObservation>, IDisposable
    {
        private readonly DriveInfo drive;

        private readonly PerformanceCounter readTimeCounter;
        private readonly PerformanceCounter writeTimeCounter;
        private readonly PerformanceCounter readRateCounter;
        private readonly PerformanceCounter writeRateCounter;

        public VolumeObserver(DriveInfo drive)
        {
            this.drive = drive ?? throw new ArgumentNullException(nameof(drive));

            var instanceName = drive.Name.Substring(0, 2);

            this.readTimeCounter  = new PerformanceCounter("LogicalDisk", "% Disk Read Time", instanceName);
            this.writeTimeCounter = new PerformanceCounter("LogicalDisk", "% Disk Write Time", instanceName);
            this.readRateCounter  = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec", instanceName);
            this.writeRateCounter = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec", instanceName);
        }

        public VolumeObservation Observe()
        {
            return new VolumeObservation(
                available       : drive.TotalFreeSpace,
                size            : drive.TotalSize,
                readTimeSample  : readTimeCounter.NextSample(),
                writeTimeSample : writeTimeCounter.NextSample(),
                readRateSample  : readRateCounter.NextSample(),
                writeRateSample : writeRateCounter.NextSample(),
                date            : DateTime.UtcNow
            );
        }

        public void Dispose()
        {
            readTimeCounter.Dispose();
            writeTimeCounter.Dispose();
            readRateCounter.Dispose();
            writeRateCounter.Dispose();
        }
    }
}
