using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [Dataset("Route")]
    public class Route : INetworkRoute
    {
        public Route() { }

        public Route(long id, string name)
        {
            Id = id;
            Name = name;
        }

        // networkId + index
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [Ascii, StringLength(63)]
        public string Name { get; }

        [Member("destinationRange")]
        [Ascii, StringLength(100)]
        public string DestinationRange { get; set; }

        [Member("nextHop")]
        public IPAddress NextHop { get; set; }
        
        [Member("priority")]
        public int Priority { get; set; }

        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.NetworkRoute;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        [IgnoreDataMember]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion
    }
}