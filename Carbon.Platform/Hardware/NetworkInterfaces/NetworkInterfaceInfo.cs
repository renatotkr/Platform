using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "NetworkInterfaces")]
    public class NetworkInterfaceInfo
    {
        [Member(1), Identity]
        public long Id { get; set; }
     
        [Member(2), Indexed] // AKA mac
        public string PhysicalAddress { get; set; }

        [Member(3)] // in octects
        public long Speed { get; set; }

        [Member(4, mutable: true), Indexed]
        public long? HostId { get; set; }

        #region Stats (5-8)

        [Member(5, mutable: true)]  // in octects
        public long DataIn { get; }

        [Member(6, mutable: true)]  // in octects
        public long DataOut { get; }

        [Member(7, mutable: true)]  // in octects
        public long PacketsIn { get; }

        [Member(8, mutable: true)]  // in octects
        public long PacketsOut { get; }

        // [Mutable]
        // public long PacketsDiscarded { get; }

        // DatagramSize (largest)

        #endregion

        public IList<IPAddress> Addresses { get; set; }
    }
}

/*
public string Description { get; set; } //  e.g. Intel(R) PRO/1000 MT Network Connection

public string InstanceName { get; set; }
*/
