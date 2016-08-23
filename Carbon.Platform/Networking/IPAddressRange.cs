using System.Net;

namespace Carbon.Networking
{
    // e.g. 10.138.0.0/20
    public struct IPAddressRange
    {
        public IPAddress Prefix { get; set; } 

        public int Cidr { get; set; } // 24

        #region Helpers

        public long Netmask => ~((1 << (32 - Cidr)) - 1);  // 255.255.255.0

        public IPAddress Broadcast => null;

        public IPAddress Start => new IPAddress(Prefix.Address & Netmask);  // 10.1.1.1
        
        public IPAddress End => new IPAddress((Prefix.Address & Netmask) | ~Netmask);    // 10.1.1.254

        #endregion

        // 10.1.1.1/24
        // 192.168.2.0/24

        public static IPAddressRange Parse(string text)
        {
            var parts = text.Split('/');

            return new IPAddressRange  {
                Prefix = IPAddress.Parse(parts[0]),
                Cidr = int.Parse(parts[1])
            };
        }
    }
}
