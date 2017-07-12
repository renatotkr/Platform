using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("LoadBalancers", Schema = "Computing")]
    [UniqueIndex("ownerId", "name")]
    [UniqueIndex("providerId", "resourceId")]
    public class LoadBalancer : ILoadBalancer
    {
        public LoadBalancer() { }

        public LoadBalancer(
            long id, 
            string name,
            string address, 
            long ownerId,
            ManagedResource resource,
            long networkId = 0)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(id));

            #endregion

            Id         = id;
            Name       = name;
            Address    = address ?? throw new ArgumentNullException(nameof(address));
            NetworkId  = networkId;
            OwnerId    = ownerId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }
        
        [Member("id"), Key(sequenceName: "loadBalancerId")]
        public long Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("address")]
        [StringLength(100)]
        public string Address { get; }

        [Member("networkId")]
        public long NetworkId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        // app/my-load-balancer/50dc6c495c0c9188

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.LoadBalancer;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}