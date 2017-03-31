using System;
using System.Net.NetworkInformation;

namespace Carbon.Platform.Monitoring
{
    public class NetworkInterfaceObserver : IObservable<NetworkInterfaceObservation>
    {
        private readonly NetworkInterface networkInterface;
     
        public NetworkInterfaceObserver(NetworkInterface networkInterface)
        {
            this.networkInterface = networkInterface ?? throw new ArgumentNullException(nameof(networkInterface));
        }

        public NetworkInterfaceObservation Observe()
        {
            return new NetworkInterfaceObservation(
                networkInterface : networkInterface,
                stats            : networkInterface.GetIPStatistics(),
                date             : DateTime.UtcNow
            );
        }
    }
}


// https://msdn.microsoft.com/en-us/library/ms803962.aspx
