using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Carbon.Platform.Extensions
{
    public static class NetworkInterfaceExtensions
    {
        public static IEnumerable<IPAddress> GetIps(this NetworkInterface ni)
        {
            foreach (var ip in ni.GetIPProperties().UnicastAddresses)
            {
                // Skip the loopback address
                if (IPAddress.IsLoopback(ip.Address)) continue;

                // Skip non-IE4 addresses
                if (ip.Address.AddressFamily != AddressFamily.InterNetwork) continue;

                // Skip the 169 family
                if (ip.Address.ToString().StartsWith("169.")) continue;

                yield return ip.Address;
            }
        }

    }
}