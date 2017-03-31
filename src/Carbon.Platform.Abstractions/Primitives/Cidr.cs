using System;
using System.Net;

namespace Carbon.Net
{
    using Extensions;

    /// <summary>
    /// Classless Inter-Domain Routing
    /// </summary>
    public struct Cidr
    {
        public Cidr(IPAddress prefix, int suffix)
        {
            Prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
            Suffix = suffix;
        }

        public IPAddress Prefix { get; } 

        public int Suffix { get; } // 24

        #region Helpers

        public long Netmask 
            => ~((1 << (32 - Suffix)) - 1);  // 255.255.255.0

        public IPAddress Broadcast => null;

        public IPAddress Start =>
            new IPAddress(IP4Number(Prefix) & Netmask);  // 10.1.1.1
        
        public IPAddress End => 
            new IPAddress((IP4Number(Prefix) & Netmask) | ~Netmask);    // 10.1.1.254

        private static int IP4Number(IPAddress ip) => 
            BitConverter.ToInt32(ip.GetAddressBytes(), 0);

        #endregion

        // 10.138.0.0/20    
        // 192.168.2.0/24
        // 192.168.100.14/24
        // 2001:db8::/32

        public static Cidr Parse(string text)
        {
            var parts = text.Split(Seperators.ForwardSlash); // '/'

            return new Cidr(
                prefix: IPAddress.Parse(parts[0]),
                suffix: int.Parse(parts[1])
            );
        }

        public override string ToString()
        {
            return Prefix.ToString() + "/" + Suffix.ToString();
        }
    }
}

