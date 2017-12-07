using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Computing
{
    [Dataset("LoadBalancerRules", Schema = "Computing")]
    public class LoadBalancerRule : ILoadBalancerRule
    {
        public LoadBalancerRule() { }

        public LoadBalancerRule(
            long id, 
            string condition,
            string action, 
            int priority,
            string resourceId = null)
        {
            Validate.Id(id);
            Validate.NotNullOrEmpty(condition, nameof(condition));
            Validate.NotNullOrEmpty(action, nameof(action));
          
            Id         = id;
            Condition  = condition ;
            Action     = action;
            Priority   = priority;
            ResourceId = resourceId;
        }

        // loadBalancerId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("condition")]
        [StringLength(1000)]
        public string Condition { get; }

        [Member("action")]
        [Ascii, StringLength(100)]
        public string Action { get; }

        [Member("priority")]
        public int Priority { get; }

        public long LoadBalancerId => ScopedId.GetScope(Id);

        #region IResource

        [Member("resourceId")]
        [StringLength(150)]
        public string ResourceId { get; }
        
        ResourceType IResource.ResourceType => ResourceTypes.LoadBalancerRule;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion
    }
}