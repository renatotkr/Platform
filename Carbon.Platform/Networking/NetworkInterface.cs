using System.Collections.Generic;
using System.Net;

namespace Carbon.Computing
{
    using Data.Annotations;

    [Dataset("NetworkInterfaces")]
    public class NetworkInterface
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2)]
        public long NetworkId { get; set; }

        [Member(2), Indexed] // AKA mac
        public string PhysicalAddress { get; set; }

        [Member(3)] // in octects
        public long Speed { get; set; }

        [Member(4), Mutable, Indexed]
        public long? HostId { get; set; }

        #region Stats (5-8)

        [Member(5), Mutable]  // in octects
        public long NetworkIn { get; }

        [Member(6), Mutable]  // in octects
        public long NetworkOut { get; }

        [Member(7), Mutable]  // in octects
        public long PacketsIn { get; }

        [Member(8), Mutable]  // in octects
        public long PacketsOut { get; }

        // [Mutable]
        // public long PacketsDiscarded { get; }

        // DatagramSize (largest)

        #endregion

        public string InstanceName { get; set; }

        public IList<IPAddress> Addresses { get; set; }
    }
}

/*
public string Description { get; set; } //  e.g. Intel(R) PRO/1000 MT Network Connection

public string InstanceName { get; set; }
*/
