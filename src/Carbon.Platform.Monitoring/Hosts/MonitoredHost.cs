using System.Collections.Generic;

using Carbon.Platform.Computing;
using Carbon.Platform.Networking;

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