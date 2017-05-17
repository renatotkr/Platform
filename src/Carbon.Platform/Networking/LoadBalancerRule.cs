using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Networking
{
    [Dataset("LoadBalancerRules")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class LoadBalancerRule : ILoadBalancerRule
    {
        public LoadBalancerRule() { }

        public LoadBalancerRule(
            long id, 
            string condition,
            string action, 
            int priority,
            ManagedResource resource)
        {
            Id         = id;
            Condition  = condition ?? throw new ArgumentNullException(nameof(condition));
            Action     = action ?? throw new ArgumentNullException(nameof(action));
            Priority   = priority;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
        }

        // loadBalancerId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("condition")]
        [StringLength(500)]
        public string Condition { get; }

        [Member("action")]
        public string Action { get; }

        [Member("priority")]
        public int Priority { get; }

        public long LoadBalancerId => ScopedId.GetScope(Id);

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(120)]
        public string ResourceId { get; }
        
        ResourceType IResource.ResourceType => ResourceTypes.LoadBalancerRule;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
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