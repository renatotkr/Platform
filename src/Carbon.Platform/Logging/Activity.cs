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

        public Activity(
            string action,
            IResource resource, 
            JsonObject context = null)
        {
            #region Preconditions

            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            #endregion

            Action     = action ?? throw new ArgumentNullException(nameof(action));
            Resource   = resource.ResourceType.ToString() + "#" + resource.Id;
            Context    = context;
        }

        // change to bigId (accountId | timestamp | sequenceNumber)
        [Member("id"), Key(sequenceName: "activityId", cacheSize: 1000)]
        public long Id { get; set; }

        // e.g. create
        [Member("action")]
        public string Action { get; }
        
        // e.g. host#4234524
        [Member("resource")]
        public string Resource { get; }

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

// Bringt this up to IAM layer