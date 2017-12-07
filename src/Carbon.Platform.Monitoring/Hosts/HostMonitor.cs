using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{
    public class HostMonitor
    {
        private readonly TimeSpan interval;

        private readonly List<ResourceMonitor> monitors = new List<ResourceMonitor>();

        private readonly Action<IReadOnlyList<MetricData>> reportAction;

        private readonly AutoResetEvent stopEvent = new AutoResetEvent(false);

        public HostMonitor(
            MonitoredHost host,
            TimeSpan interval, 
            Action<IReadOnlyList<MetricData>> reportAction)
        {
            var localhost = Localhost.Get();

            // Coming in 4.5 release

            var hostDimension = new Dimension("hostId", host.Id);

            /*

            // Processor Counters https://technet.microsoft.com/en-us/library/cc938593.aspx

            counters.Add(new WindowsMonitor(KnownMetrics.ProcessorUserTime,   new[] { hostDimension }, new PerformanceCounter("Processor", "% User Time",           "_Total")));
            counters.Add(new WindowsMonitor(KnownMetrics.ProcessorSystemTime, new[] { hostDimension }, new PerformanceCounter("Processor", "% Privileged Time",     "_Total")));
            */

            /*
            for (int i = 0; i < localhost.Drives.Length; i++)
            {
                var drive = localhost.Drives[i];

                var instanceName = drive.Name.Substring(0, 2); // e.g. C:

                Dimension[] tags;

                if (host.Volumes.Length > 0)
                {
                    var volume = host.Volumes[i];

                    tags = new[] { hostDimension, new Dimension("volumeId", volume.Id) };
                }
                else
                {
                    tags = new[] { hostDimension };
                }

                // counters.Add(new WindowsMonitor(KnownMetrics.VolumeReadTime, tags, new PerformanceCounter("LogicalDisk", "% Disk Read Time", instanceName)));
                // counters.Add(new WindowsMonitor(KnownMetrics.VolumeWriteTime, tags, new PerformanceCounter("LogicalDisk", "% Disk Write Time", instanceName)));
                // counters.Add(new WindowsMonitor(KnownMetrics.VolumeReadOperations, tags, new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec", instanceName)));
                // counters.Add(new WindowsMonitor(KnownMetrics.VolumeWriteOperations, tags, new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec", instanceName)));

                monitors.Add(new AbsoluteValueMonitor(MetricNames.VolumeAvailableBytes, tags, () => drive.AvailableFreeSpace));
                monitors.Add(new AbsoluteValueMonitor(MetricNames.VolumeTotalBytes,     tags, () => drive.TotalSize));
            }
            */
            
            for (int i = 0; i < localhost.NetworkInterfaces.Length; i++)
            {
                var nic = localhost.NetworkInterfaces[i];

                Dimension[] tags;

                var netResource = host.NetworkInterfaces?.FirstOrDefault(ni =>
                    Enumerable.SequenceEqual(ni.MacAddress.GetAddressBytes(), nic.GetPhysicalAddress().GetAddressBytes())
                );

                if (netResource == null) continue;

                if (netResource != null)
                {
                    tags = new[] { hostDimension, new Dimension("interfaceId", netResource.Id) };
                }
                else
                {
                    tags = new[] { hostDimension };
                }

                // TODO: Map by MacAddress

                monitors.Add(new NetworkInterfaceMonitor(tags, nic));
            }
            

            this.interval = interval;
            this.reportAction = reportAction;
        }

        private Task task;
        bool isRunning = false;

        public void Start()
        {
            isRunning = true;

            task = Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
        }

        private void Run()
        {
            while (isRunning)
            {
                stopEvent.WaitOne(interval);
                
                Next();        
            }
        }
        
        private void Next()
        {
            // TODO: Observe simultaneously
            var batch = new List<MetricData>(monitors.Count);

            foreach (var counter in monitors)
            {
                try
                {
                    batch.AddRange(counter.Observe());   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error observing {counter.GetType().Name}:" + ex.Message );
                }
            }
            
            reportAction(batch);
        }

        public async Task StopAsync()
        {
            foreach (var counter in monitors)
            {
                counter.Dispose();
            }

            isRunning = false;

            stopEvent.Set();

            await task.ConfigureAwait(false);

            stopEvent.Dispose();
        }
    }
}