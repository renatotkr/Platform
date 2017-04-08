using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkRoutes")]
    public class NetworkRoute : INetworkRoute
    {
        public NetworkRoute() { }

        public NetworkRoute(long id)
        {
            Id = id;
        }

        // networkId + index
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; set; }

        [Member("destinationRange")]
        public string DestinationRange { get; set; }

        [Member("nextHop")]
        public IPAddress NextHop { get; set; }
        
        [Member("priority")]
        public int Priority { get; set; }

        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);

        #region Timestamps

        [Member("created"), Timestamp]
        [IgnoreDataMember]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}