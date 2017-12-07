
using Carbon.Platform.Networking;
using Carbon.Platform.Storage;

namespace Carbon.Platform.Monitoring
{
    public class MonitoredHost
    {
        public MonitoredHost(
            long id, 
            INetworkInterface[] networkInterfaces,
            IVolume[] volumes)
        {
            Id                = id;
            NetworkInterfaces = networkInterfaces;
            Volumes           = volumes;
        }

        public long Id { get; }

        public INetworkInterface[] NetworkInterfaces { get; }

        public IVolume[] Volumes { get; }
    }
}