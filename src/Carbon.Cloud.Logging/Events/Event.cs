using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;
using Carbon.Security;

namespace Carbon.Cloud.Logging
{
    [Dataset("Events", Schema = "Logging")]
    public class Event
    {
        public Event() { }

        public Event(
            string action,
            string resource,
            ISecurityContext context,
            JsonObject properties = null)
        {
            Action     = action   ?? throw new ArgumentNullException(nameof(action));
            Resource   = resource ?? throw new ArgumentNullException(nameof(resource));
            Properties = properties;
            UserId     = context?.UserId;
        }

        // accountId | timestamp | #
        [Member("id"), Key]
        public Uid Id { get; set; }

        [Member("action")]
        [StringLength(100)]
        public string Action { get; }

        [Member("resource"), Indexed]
        [StringLength(150)]
        public string Resource { get; }

        // origin?
        [Member("source"), Optional]
        [StringLength(120)]
        public string Source { get; }

        [Member("properties"), Optional]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        [Member("context"), Optional]
        [StringLength(1000)]
        public JsonObject Context { get; }

        [Member("userId"), Indexed]
        public long? UserId { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public DateTime Created => RequestId.GetTimestamp(Id);

        #endregion
    }
}

// ACTION  | RESOURCE
// launch  | carbon:host/1
// restart | carbon:host/1
// view    | https://carbon/