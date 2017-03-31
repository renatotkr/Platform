using System;
using System.Net.NetworkInformation;

namespace Carbon.Platform.Monitoring
{
    public class NetworkInterfaceObservation
    {
        public NetworkInterfaceObservation(NetworkInterface networkInterface, IPInterfaceStatistics stats, DateTime date)
        {
            NetworkInterface = networkInterface ?? throw new ArgumentNullException(nameof(networkInterface));
            Stats            = stats ?? throw new Exception(nameof(stats));
            Date             = date;
        }

        public NetworkInterface NetworkInterface { get; }

        public IPInterfaceStatistics Stats { get; }

        public DateTime Date { get; }
    }
}