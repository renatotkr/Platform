using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkRouters")]
    public class NetworkRouter : INetworkRouter
    {
        public NetworkRouter() { }

        public NetworkRouter(long id, string name, ManagedResource resource)
        {
            Id         = id;
            Name       = name;
            ProviderId = resource.Provider.Id;
            LocationId = resource.LocationId;
            ResourceId = resource.Id;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        public string Name { get; }

        public long NetworkId => ScopedId.GetScope(Id);

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [IgnoreDataMember]
        [Member("locationId")]
        public long LocationId { get; }

        ResourceType IManagedResource.ResourceType => ResourceType.NetworkRouter;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}