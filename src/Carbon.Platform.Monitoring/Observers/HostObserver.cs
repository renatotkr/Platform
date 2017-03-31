using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carbon.Platform.Monitoring
{
    public class HostObserver : IObservable<HostObserveration>, IDisposable
    {
        private readonly Localhost host;
        private readonly NetworkInterfaceObserver[] networkInterfaces;
        private readonly VolumeObserver[] volumes;
        private readonly ProcessorObserver[] processors;

        public HostObserver(Localhost host)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));

            this.processors         = GetProcessorObservers();
            this.networkInterfaces  = GetNetworkInterfaceObservers().ToArray();
            this.volumes            = GetVolumeObservers(host.Drives);
        }

        public HostObserveration Observe()
        {
            var memory = MemoryInfo.Observe();

            return new HostObserveration(
                hostId            : 1,
                apps              : Array.Empty<AppObservation>(),
                networkInterfaces : ObserveNetworkInterfaces(),
                memory            : new MemoryStats(memory.Used, memory.Total),
                processors        : ObserverProcessors(),
                volumes           : ObserveVolumes(),
                timestamp         : DateTime.UtcNow
            );
        }

        public ProcessorObservation[] ObserverProcessors()
        {
            var observerations = new ProcessorObservation[processors.Length];

            for (var i = 0; i < observerations.Length; i++)
            {
                observerations[i] = processors[i].Observe();
            }

            return observerations;
        }

        public VolumeObservation[] ObserveVolumes()
        {
            var observerations = new VolumeObservation[volumes.Length];

            for (var i = 0; i < observerations.Length; i++)
            {
                observerations[i] = volumes[i].Observe();
            }

            return observerations;
        }

        public NetworkInterfaceObservation[] ObserveNetworkInterfaces()
        {
            var observerations = new NetworkInterfaceObservation[networkInterfaces.Length];

            for (var i = 0; i < observerations.Length; i++)
            {
                observerations[i] = networkInterfaces[i].Observe();
            }

            return observerations;
        }

        public void Dispose()
        {
            foreach (var observer in processors)
            {
                observer.Dispose();
            }

            foreach (var observer in volumes)
            {
                observer.Dispose();
            }
        }

        private ProcessorObserver[] GetProcessorObservers()
        {
            var observers = new ProcessorObserver[Environment.ProcessorCount];

            for (var i = 0; i < observers.Length; i++)
            {
                observers[i] = new ProcessorObserver(i.ToString());
            }

            return observers;
        }

        private VolumeObserver[] GetVolumeObservers(IReadOnlyList<DriveInfo> drives)
        {
            var observers = new VolumeObserver[host.Drives.Count];

            for (var i = 0; i < host.Drives.Count; i++)
            {
                observers[i] = new VolumeObserver(host.Drives[i]);
            }

            return observers;
        }

        private IEnumerable<NetworkInterfaceObserver> GetNetworkInterfaceObservers()
        {
            foreach (var ni in host.NetworkInterfaces)
            {
                var observer = new NetworkInterfaceObserver(ni);

                observer.Observe(); // Make an initial observersation to make sure things are OK

                yield return observer;
            }
        }


        /*
        private IEnumerable<AppMonitor> MonitorApps(HostInfo host)
        {
            using (var iis = new IisAppHost(host, new AppHostConfiguration(), log))
            {
                foreach (var appInstance in iis.Scan().Where(app => !app.AppName.Contains(" ")))
                {
                    var appObserver = new AppInstanceObserver(appInstance);

                    if (appObserver.Observe() != null)
                    {
                        log.Info("*" + appInstance.AppName);
                    }
                    else
                    {
                        log.Info(appInstance.AppName);

                        if (appObserver.Fault != null)
                        {
                            log.Info("FAULT:" + appObserver.Fault.Message);
                        }
                    }

                    log.Info($"Monitoring app " + appInstance.AppName);

                    yield return appObserver;
                }
            }
        }
        */
    }
}