using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Logging
{
    [Dataset("Activities")]
    public class Activity
    {
        public Activity() { }

        public Activity(
            string action,
            IResource resource, 
            JsonObject context = null)
        {
            #region Preconditions

            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            #endregion

            Action       = action ?? throw new ArgumentNullException(nameof(action));
            ResourceType = resource.ResourceType.Name;
            ResourceId    = resource.Id;
            Context       = context;
        }

        // change to bigId (accountId | timestamp | sequenceNumber)
        [Member("id"), Key(sequenceName: "activityId", cacheSize: 1000)]
        public long Id { get; set; }

        // e.g. create
        [Member("action")]
        public string Action { get; }
        
        // e.g. host, account, etc.
        [Member("resourceType")]
        [StringLength(30)]
        public string ResourceType { get; }

        [Member("resourceId")]
        public long ResourceId { get; set; }

        [Member("context"), Optional]
        [StringLength(1000)]
        public JsonObject Context { get; }

        [Member("sessionId")]
        public long? SessionId { get; set; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }
}

// TODO: Bring up to IAM layer