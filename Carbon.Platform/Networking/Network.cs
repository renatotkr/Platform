using System.Collections.Generic;
using System.Net;

namespace Carbon.Networking
{
    using Data.Annotations;

    [Dataset("Networks", Schema = "Networking")]
    public class Network
    {
        [Member(1)]
        public long Id { get; set; }

        [Member(2)] // 10.1.1.0
        public IPAddress Prefix { get; set; } 

        [Member(3)]
        public int Cidr { get; set; } // 24

        [Member(4)]
        public long ZoneId { get; set; }

        [Member(5)] // So we don't have to lookup through zone...
        public long ProviderId { get; set; }

        [Member(5)] // Provides WAN access
        public IPAddress Gateway { get; set; } // 10.1.1.0

        [Member(6)] // Networks may be divided...
        public long? ParentId { get; set; }

        [Member(7)]
        public long OwnerId { get; set; }

        [Member(8)] // AS226 (Autonomous System #)
        public int? ASNumber { get; set; }

        #region Helpers

        public long Netmask => ~((1 << (32 - Cidr)) - 1);  // 255.255.255.0

        public IPAddress Broadcast => null;

        public IPAddress Start => new IPAddress(Prefix.Address & Netmask);  // 10.1.1.1
        
        public IPAddress End => new IPAddress((Prefix.Address & Netmask) | ~Netmask);    // 10.1.1.254

        #endregion

        // The rules form the firewall
        public IList<NetworkRule> Rules { get; set; }

        // Tells packets where to go leaving a network interface
        public IList<NetworkRoute> Routes { get; set; }

        public static Network Parse(string text)
        {
            var parts = text.Split('/');

            return new Network  {
                Prefix = IPAddress.Parse(parts[0]),
                Cidr = int.Parse(parts[1])
            };
        }
    }
}


/*
us-west1	    10.138.0.0/20	10.138.0.1
us-central1	    10.128.0.0/20	10.128.0.1
us-east1	    10.142.0.0/20	10.142.0.1
europe-west1	10.132.0.0/20	10.132.0.1
asia-east1	    10.140.0.0/20	10.140.0.1
*/
