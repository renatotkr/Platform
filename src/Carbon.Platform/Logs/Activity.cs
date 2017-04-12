using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Logs
{
    [Dataset("Activities")]
    public class Activity
    {
        public Activity() { }

        public Activity(ActivityType type, IResource resource)
        {
            ResourceType = resource.ResourceType;
            ResourceId   = resource.Id;
            Type         = type;
        }

        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("type")]
        public ActivityType Type { get; }

        [Member("resourceType")]
        public ResourceType ResourceType { get; }

        [Member("resourceId")]
        [Indexed]
        public long ResourceId { get; }

        [Member("details"), Optional]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }
}