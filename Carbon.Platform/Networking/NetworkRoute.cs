using System.Net;

namespace Carbon.Networking
{
    using Data.Annotations;

    [Record(TableName = "NetworkRoutes")]
    public class NetworkRoute
    {
        [Member(1), Key]
        public long NetworkId { get; set; }

        [Member(2), Key]
        public long Id { get; set; }

        [Member(3)]
        public IPAddressRange DestinationRange { get; set; }

        [Member(4)]
        public int Priority { get; set; }

        [Member(5)]
        public IPAddress NextHop { get; set; }
    }
}

/*
NAME                           NETWORK   DEST_RANGE    NEXT_HOP                 PRIORITY
default-route-02a98b9a14f7edc4 default   10.128.0.0/20                          1000
default-route-081fa300345dd52a default   0.0.0.0/0     default-internet-gateway 1000
default-route-93a38d78c77eac66 default   10.132.0.0/20                          1000
default-route-999664b72dd247e7 default   10.140.0.0/20                          1000
default-route-a1f15d0858cd51e1 default   10.142.0.0/20                          1000
*/

// A subnet network divides your global network in to regional subnets, each with its own IPv4 prefix. 
// Instead, each subnetwork has an IP range and gateway address. 
