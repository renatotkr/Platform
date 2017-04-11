using System;
using System.Collections.Generic;
using System.Linq;

#if NET461
using System.Diagnostics;
#endif

using System.Threading;
using System.Threading.Tasks;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{

   
    public class HostMonitor
    {
        private readonly TimeSpan interval;

        private readonly List<IMonitor> counters = new List<IMonitor>();

        private readonly Action<IReadOnlyList<MetricData>> action;

        private readonly AutoResetEvent stopEvent = new AutoResetEvent(false);

        public HostMonitor(
            MonitoredHost host,
            TimeSpan interval, 
            Action<IReadOnlyList<MetricData>> action)
        {
            var localhost = Localhost.Get();

#if NET461
            var pTags = new[] { new Dimension("hostId", host.Id) };

            // Processor Counters https://technet.microsoft.com/en-us/library/cc938593.aspx

            counters.Add(new WindowsMonitor(KnownMetrics.ProcessorUserTime,   pTags, new PerformanceCounter("Processor", "% User Time",           "_Total")));
            counters.Add(new WindowsMonitor(KnownMetrics.ProcessorSystemTime, pTags, new PerformanceCounter("Processor", "% Privileged Time",     "_Total")));

            for (int i = 0; i < localhost.Drives.Count; i++)
            {
                var drive = localhost.Drives[i];

                var instanceName = drive.Name.Substring(0, 2); // e.g. C:

                Dimension[] tags;

                if (host.Volumes.Count > 0)
                {
                    var volume = host.Volumes[i];

                    tags = new[] { new Dimension("hostId", host.Id), new Dimension("volumeId", volume.Id) };
                }
                else
                {
                    tags = new[] { new Dimension("hostId", host.Id), };
                }

                counters.Add(new WindowsMonitor(KnownMetrics.VolumeReadTime,        tags, new PerformanceCounter("LogicalDisk", "% Disk Read Time",     instanceName)));
                counters.Add(new WindowsMonitor(KnownMetrics.VolumeWriteTime,       tags, new PerformanceCounter("LogicalDisk", "% Disk Write Time",    instanceName)));
                counters.Add(new WindowsMonitor(KnownMetrics.VolumeReadOperations,  tags, new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec",  instanceName)));
                counters.Add(new WindowsMonitor(KnownMetrics.VolumeWriteOperations, tags, new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec", instanceName)));
                
                counters.Add(new AbsoluteValueMonitor(KnownMetrics.VolumeAvailableBytes, tags, () => drive.AvailableFreeSpace));
                counters.Add(new AbsoluteValueMonitor(KnownMetrics.VolumeTotalBytes, tags, () => drive.TotalSize));
            }

#endif
            for (int i = 0; i < localhost.NetworkInterfaces.Count; i++)
            {
                var nic = localhost.NetworkInterfaces[i];

                Dimension[] tags;

                var netResource = host.NetworkInterfaces?.FirstOrDefault(ni =>
                    Enumerable.SequenceEqual(ni.MacAddress.GetAddressBytes(), nic.GetPhysicalAddress().GetAddressBytes())
                );

                if (netResource != null)
                {
                    tags = new[] { new Dimension("hostId", host.Id), new Dimension("interfaceId", netResource.Id) };
                }
                else
                {
                    tags = new[] { new Dimension("hostId", host.Id), };
                }

                // TODO: Map by MacAddress

                counters.Add(new NetworkInterfaceMonitor(tags, nic));
            }

            this.interval = interval;
            this.action = action;

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
            var batch = new List<MetricData>();

            foreach (var counter in counters)
            {
                try
                {
                    foreach (var result in counter.Observe())
                    {
                        batch.Add(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error observing {counter.GetType().Name}:" + ex.Message );
                }
            }
            
            action(batch);
        }

        public void Stop()
        {
            foreach (var counter in counters)
            {
                counter.Dispose();
            }

            isRunning = false;

            stopEvent.Set();

            task.Wait();

            stopEvent.Dispose();

        }
    }
}