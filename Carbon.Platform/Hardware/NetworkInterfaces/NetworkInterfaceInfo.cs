using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "NetworkInterfaces")]
    public class NetworkInterfaceInfo
    {
        [Identity]
        public long Id { get; set; }
     
        [Indexed] // AKA mac
        public string PhysicalAddress { get; set; }

        [Mutable] // in octects
        public long Speed { get; set; }

        [Mutable] // in octects
        public long DataIn { get; }

        [Mutable] // in octects
        public long DataOut { get; }

        [Mutable] // in octects
        public long PacketsIn { get; }

        [Mutable] // in octects
        public long PacketsOut { get; }

        // [Mutable]
        // public long PacketsDiscarded { get; }

        #region Helpers

        /// <summary>
        /// e.g. Intel(R) PRO/1000 MT Network Connection
        /// </summary>
        [Exclude]
        public string Description { get; set; }

        [Exclude]
        public string InstanceName { get; set; }

        // IP Addresses
        [Exclude]
        public IList<IPAddress> Addresses { get; set; }

        #endregion


        // largest datagram size
    }
}