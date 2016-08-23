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

        [Member(4, Mutable = true), Indexed]
        public long? HostId { get; set; }

        #region Stats (5-8)

        [Member(5, Mutable = true)]  // in octects
        public long DataIn { get; }

        [Member(6, Mutable = true)]  // in octects
        public long DataOut { get; }

        [Member(7, Mutable = true)]  // in octects
        public long PacketsIn { get; }

        [Member(8, Mutable = true)]  // in octects
        public long PacketsOut { get; }

        // [Mutable]
        // public long PacketsDiscarded { get; }

        // DatagramSize (largest)

        #endregion

        public IList<IPAddress> Addresses { get; set; }
    }
}

/*
/// <summary>
/// e.g. Intel(R) PRO/1000 MT Network Connection
/// </summary>
[Exclude]
public string Description { get; set; }

[Exclude]
public string InstanceName { get; set; }
*/