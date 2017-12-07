using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Cloud.Logging
{
    [Dataset("Events", Schema = "Logs")]
    public class Event
    {
        public Event() { }

        public Event(
            string action,
            string resource,
            long? userId = null,
            string source = null,
            JsonObject context = null,
            JsonObject properties = null)
        {
            Action     = action   ?? throw new ArgumentNullException(nameof(action));
            Resource   = resource ?? throw new ArgumentNullException(nameof(resource));
            Context    = context;
            Properties = properties;
            Source     = source;
            UserId     = userId;
        }

        // accountId | timestamp | #
        [Member("id"), Key]
        public Uid Id { get; set; }

        [Member("action")]
        [Ascii, StringLength(50)]
        public string Action { get; } // View, Invoke, ...

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
        public long? UserId { get; }
        
        // SessionId?

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