using System;

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
            long ownerId,
            string address,
            ManagedResource resource,
            long networkId = 0,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (string.IsNullOrEmpty(address))
                throw new ArgumentException("Required", nameof(address));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id = id;
            Name       = name;
            Address    = address;
            NetworkId  = networkId;
            OwnerId    = ownerId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
            Properties = properties ?? new JsonObject();
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

        #region Stats

        [Member("listenerCount")]
        public int ListenerCount { get; }

        [Member("ruleCount")]
        public int RuleCount { get; }

        #endregion

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        // app/my-load-balancer/50dc6c495c0c9188

        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.LoadBalancer;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}