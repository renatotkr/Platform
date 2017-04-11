using System.Collections.Generic;

using Carbon.Platform.Networking;
using Carbon.Platform.Storage;

namespace Carbon.Platform.Monitoring
{
    public class MonitoredHost
    {
        public MonitoredHost(
            long id, 
            IReadOnlyList<INetworkInterface> networkInterfaces,
            IReadOnlyList<IVolume> volumes)
        {
            Id = id;
            NetworkInterfaces = networkInterfaces;
            Volumes = volumes;
        }

        public long Id { get; }

        public IReadOnlyList<INetworkInterface> NetworkInterfaces { get; }

        public IReadOnlyList<IVolume> Volumes { get; }
    }
}