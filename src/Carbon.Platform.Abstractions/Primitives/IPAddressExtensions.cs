using System;
using System.Net;

namespace Carbon.Extensions
{
    public static class IPAddressExtensions
    {
        public static int IP4Number(this IPAddress ip)
        {
            return BitConverter.ToInt32(ip.GetAddressBytes(), 0);
        }

        // http://stackoverflow.com/a/39120248
        public static bool IsInternal(this IPAddress address)
        {
            var bytes = address.GetAddressBytes();

            switch (bytes[0])
            {
                // 10.0.0.0 - 10.255.255.255 (10/8 prefix)
                case 10     : return true;

                // 172.16.0.0 - 172.31.255.255 (172.16/12 prefix)
                case 172    : return bytes[1] < 32 && bytes[1] >= 16;

                // 192.168.0.0 - 192.168.255.255 (192.168/16 prefix)
                case 192    : return bytes[1] == 168;

                default     : return false;
            }
        }
    }
}
