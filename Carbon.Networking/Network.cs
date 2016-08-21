using System.Net;

namespace Carbon.Platform
{
    public class Network
    {
        public long Id { get; }

        public IPAddress Start { get; set; }

        public IPAddress End { get; set; }
    }

    // AddressRange
}
