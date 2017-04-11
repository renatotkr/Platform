using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Apps;

namespace Carbon.Platform.Logs
{
    [Dataset("Activities")]
    public class Activity
    {
        public Activity() { }

        public Activity(IApp app, ActivityType type)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            #endregion

            ResourceType = ResourceType.App;
            ResourceId   = app.Id;
            Type         = type;
        }

        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("type")]
        public ActivityType Type { get; }

        [Member("resourceType")]
        public ResourceType ResourceType { get; set; }

        [Member("resourceId")]
        [Indexed]
        public long ResourceId { get; set; }

        [Member("details"), Optional]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }

    public enum ActivityType
    {
        Create   = 1,
        Build    = 2,
        Publish  = 3,  // New Version
        Deploy   = 4,  // On instance

        Add      = 7, // Instance
        Remove   = 8, // Instance
    }
}